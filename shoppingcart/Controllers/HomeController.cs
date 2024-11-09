using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;
using shoppingcart.Repositories;
using System.Diagnostics;

namespace shoppingcart.Controllers
{
	public class HomeController : Controller
	{
		private readonly DataContext _dataContext; 
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger , DataContext _context)
		{
			_logger = logger;
			_dataContext = _context;
		}

		public IActionResult Index()
		{
			var products = _dataContext.Products.Include("Category").Include("Brand").ToList();
			return View(products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statuscode)
		{
			if (statuscode == 404)
			{
				return View("NotFound");
			}
			else {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
	}
}
