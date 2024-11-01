using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;
using shoppingcart.Repositories;

namespace shoppingcart.Controllers
{
	public class CategoryController : Controller
	{

		private readonly DataContext _dataContext;

		public CategoryController(DataContext _context) {
			_dataContext = _context;
		}
		public async Task<IActionResult> Index(string Slug="")
		{
			CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if (category == null) return RedirectToAction("Index");
			var productsByCategory = _dataContext.Products.Where(c => c.CategoryId == category.Id);
			return View(await productsByCategory.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
