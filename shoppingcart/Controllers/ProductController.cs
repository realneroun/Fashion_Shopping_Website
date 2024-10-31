using Microsoft.AspNetCore.Mvc;

namespace shoppingcart.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details() { 
			return View("Views/Product/Details.cshtml");
		}

	}
}
