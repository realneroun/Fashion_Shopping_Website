using Microsoft.AspNetCore.Mvc;

namespace shoppingcart.Controllers
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult CheckOut()
		{
			return View("Views/Checkout/Index.cshtml");
		}
	}
}
