using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace shoppingcart.Repositories.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public CategoriesViewComponent(DataContext _context)
		{
			_dataContext = _context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _dataContext.Categories.ToListAsync());
		}
	}
}
