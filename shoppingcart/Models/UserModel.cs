﻿using System.ComponentModel.DataAnnotations;

namespace shoppingcart.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập username")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage = "Vui lòng nhập password")]
		public string Password { get; set; }		
	}
}
