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
    [Route("Admin/Category")]
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		//public async Task<IActionResult> Index()
		//{

		//	return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		//}

		[Route("Index")]
		public async Task<IActionResult> Index(int pg = 1)
		{
			List<CategoryModel> category = _dataContext.Categories.ToList(); //33 datas


			const int pageSize = 2; //10 items/trang

			if (pg < 1) //page < 1;
			{
				pg = 1; //page ==1
			}
			int recsCount = category.Count(); //33 items;

			var pager = new Paginate(recsCount, pg, pageSize);

			int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

			//category.Skip(20).Take(10).ToList()

			var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Pager = pager;

			return View(data);
		}
		[Route("Create")]
		public IActionResult Create()
        {
            return View("Create");
        }
		[Route("Create")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel objCategory)
        {
            if (ModelState.IsValid)
            {
                objCategory.Slug = objCategory.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == objCategory.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(objCategory);
                }
                _dataContext.Add(objCategory);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm category thành công";
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
            return View(objCategory);
        }
		[Route("Edit")]
		public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel objCategory = await _dataContext.Categories.FindAsync(Id);
            return View(objCategory);
        }
		[Route("Edit")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, CategoryModel objCategory)
        {
            var exist_category = _dataContext.Categories.Find(objCategory.Id);
            if (ModelState.IsValid)
            {
                exist_category.Slug = objCategory.Name.Replace(" ", "-");
                //var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == objProduct.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                //    return View(objProduct);
                //}
                exist_category.Name = objCategory.Name;
                exist_category.Description = objCategory.Description;
                exist_category.Status = objCategory.Status;
                _dataContext.Update(exist_category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit danh mục thành công";
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
            return View(objCategory);
        }
		[Route("Delete")]
		public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel objCategory = await _dataContext.Categories.FindAsync(Id);
            _dataContext.Remove(objCategory);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Xóa danh mục thành công";
            return RedirectToAction("Index");
        }
    }
}
