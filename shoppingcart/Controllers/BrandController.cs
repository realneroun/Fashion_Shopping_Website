using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;
using shoppingcart.Repositories;

namespace shoppingcart.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController (DataContext _context)
		{
			_dataContext = _context;
		}

		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productsByBrand = _dataContext.Products.Where(c => c.BrandId == brand.Id);
			return View(await productsByBrand.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
