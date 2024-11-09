using Microsoft.AspNetCore.Identity;

namespace shoppingcart.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation {  get; set; }
		public string RoleId { get; set; }
	}
}
