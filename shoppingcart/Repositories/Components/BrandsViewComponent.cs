using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace shoppingcart.Repositories.Components
{
	public class BrandsViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public BrandsViewComponent(DataContext _context)
		{
			_dataContext = _context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _dataContext.Brands.ToListAsync());
		}
	}
}
