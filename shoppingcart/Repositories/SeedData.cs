using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;

namespace shoppingcart.Repositories
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook" , Slug = "macbook" , Description = "macbook is gay", Status = 1};
				CategoryModel pc = new CategoryModel { Name = "Pc" , Slug = "pc" , Description = "pc is gay", Status = 1};

				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "apple is gay", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "samsung is gay", Status = 1 };
				_context.Products.AddRange(
					new ProductModel { Name = "Macbook", Slug = "macbook" , Description = "macbook is gay" , Image = "1.jpg" , Category = macbook , Price = 1234 , Brand = apple},
					new ProductModel { Name = "Pc", Slug = "pc" , Description = "pc is gay" , Image = "1.jpg" , Category = pc , Price = 1234 , Brand = samsung}
				);
			}
			_context.SaveChanges();
		}
	}
}
