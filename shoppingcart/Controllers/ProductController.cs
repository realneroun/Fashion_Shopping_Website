using Microsoft.AspNetCore.Mvc;
using shoppingcart.Repositories;

namespace shoppingcart.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext _context) {	
			_dataContext = _context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int Id) { 
			
			if (Id == null) return RedirectToAction("Index");
			var productById = _dataContext.Products.Where(o => o.Id == Id).FirstOrDefault();
			return View(productById);
		}

	}
}
