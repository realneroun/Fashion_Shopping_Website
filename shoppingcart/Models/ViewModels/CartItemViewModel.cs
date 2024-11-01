namespace shoppingcart.Models.ViewModels
{
	public class CartItemViewModel
	{
		public IList<CartItemModel> CartItems { get; set; }
		public decimal GrandTotal { get; set; }	
	}
}
