﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shoppingcart.Areas.Admin.Repositories;
using shoppingcart.Models;
using shoppingcart.Models.ViewModels;
using shoppingcart.Repositories;
using System.Security.Claims;

namespace shoppingcart.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly IEmailSender _emailSender;
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext dataContext, IEmailSender emailSender)
		{
			_dataContext = dataContext;
			_emailSender = emailSender;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else { 
				var orderCode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = orderCode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;	
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cartItem in cartItems) {
					var orderDetail = new OrderDetails();
					orderDetail.UserName = userEmail;
					orderDetail.OrderCode = orderCode;
					orderDetail.ProductId = cartItem.ProductId;
					orderDetail.Price = cartItem.Price;
					orderDetail.Quantity = cartItem.Quantity;
					_dataContext.Add(orderDetail);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				//send mail order success
				var receiver = "haudtps34871@fpt.edu.vn";
				var subject = "Đặt hàng thành công !!!";
				var message = "Cảm ơn bạn đã đặt hàng thành công";
				await _emailSender.SendEmailAsync(receiver, subject, message);
				TempData["success"] = "Checkout successfully";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}

	}
}
