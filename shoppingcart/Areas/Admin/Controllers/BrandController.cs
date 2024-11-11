using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;
using shoppingcart.Repositories;

namespace shoppingcart.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("Admin/Brand")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
        [Route("Index")]
        public async Task<IActionResult> Index()
		{

			return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
		}
        [Route("Create")]
        public IActionResult Create()
        {
            return View("Create");
        }
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel objBrand)
        {
            if (ModelState.IsValid)
            {
                objBrand.Slug = objBrand.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == objBrand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(objBrand);
                }
                _dataContext.Add(objBrand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
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
            return View(objBrand);
        }
        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel objBrand  = await _dataContext.Brands.FindAsync(Id);
            return View(objBrand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, BrandModel objBrand)
        {
            var exist_brand = _dataContext.Brands.Find(objBrand.Id);
            if (ModelState.IsValid)
            {
				exist_brand.Slug = objBrand.Name.Replace(" ", "-");
				//var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == objProduct.Slug);
				//if (slug != null)
				//{
				//    ModelState.AddModelError("", "Sản phẩm đã có trong database");
				//    return View(objProduct);
				//}
				exist_brand.Name = objBrand.Name;
				exist_brand.Description = objBrand.Description;
				exist_brand.Status = objBrand.Status;
                _dataContext.Update(exist_brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit thương hiệu thành công";
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
            return View(objBrand);
        }
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel objBrand = await _dataContext.Brands.FindAsync(Id);
            _dataContext.Remove(objBrand);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Xóa thương hiệu thành công";
            return RedirectToAction("Index");
        }
    }
}
