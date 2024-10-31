using Microsoft.AspNetCore.Mvc;

namespace shoppingcart.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
