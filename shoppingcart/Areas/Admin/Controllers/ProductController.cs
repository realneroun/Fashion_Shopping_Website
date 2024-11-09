using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;
using shoppingcart.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace shoppingcart.Areas.Admins.Controllers

{
    [Area("Admin")]
	[Authorize]
	public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment) {
            _dataContext = context;
            _webHostEnvironment =  webHostEnvironment;
        }

        public async Task<IActionResult> Index() {
        
            return View(await _dataContext.Products.OrderByDescending(p => p.Id ).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create() {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel objProduct)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name" , objProduct.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name" ,objProduct.BrandId);
            if (ModelState.IsValid)
            {
                objProduct.Slug = objProduct.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == objProduct.Slug);
                if (slug != null) {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(objProduct);
                }
                    if(objProduct.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                        string imageName = Guid.NewGuid().ToString()+"_"+objProduct.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir,imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await objProduct.ImageUpload.CopyToAsync(fs);  
                        fs.Close();
                        objProduct.Image = imageName;
                    }
                _dataContext.Add(objProduct);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }else
            {
                TempData["error"] = "Có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMsg = string.Join("\n", errors);
                return BadRequest(errorMsg);

            };
            return View(objProduct);
        } 
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel objProduct = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", objProduct.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", objProduct.BrandId);
            return View(objProduct);  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id,ProductModel objProduct)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", objProduct.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", objProduct.BrandId);
            var exist_product = _dataContext.Products.Find(objProduct.Id);
            if (ModelState.IsValid)
            {
                objProduct.Slug = objProduct.Name.Replace(" ", "-");
                //var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == objProduct.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                //    return View(objProduct);
                //}
                if (objProduct.ImageUpload != null)
                {
                    // upload new image
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + objProduct.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    //delete old image

                    string oldExistImage = Path.Combine(uploadDir, exist_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldExistImage))
                        {
                            System.IO.File.Delete(oldExistImage);
                        }
                    }
                    catch (Exception ex) {
                        ModelState.AddModelError("", "An error when deleting old image");
                    }
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await objProduct.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    exist_product.Image = imageName;
                }
                exist_product.Name = objProduct.Name;
                exist_product.Description = objProduct.Description;
                exist_product.Price = objProduct.Price;
                exist_product.CategoryId = objProduct.CategoryId;
                exist_product.BrandId = objProduct.BrandId;
                _dataContext.Update(exist_product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMsg = string.Join("\n", errors);
                return BadRequest(errorMsg);

            };
            return View(objProduct);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel objProduct = await _dataContext.Products.FindAsync(Id);
            if (!string.Equals(objProduct.Image, "NoImage.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldFilePath = Path.Combine(uploadDir, objProduct.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            _dataContext.Remove(objProduct);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }
    }
}
