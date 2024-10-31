using Microsoft.AspNetCore.Mvc;

namespace shoppingcart.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
