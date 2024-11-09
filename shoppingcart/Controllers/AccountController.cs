using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shoppingcart.Models;
using shoppingcart.Models.ViewModels;

namespace shoppingcart.Controllers
{
	public class AccountController : Controller
	{

		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;	

		public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager) { 
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public IActionResult Login(string returnUrl) 
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel userLoginVM)
		{
			if (ModelState.IsValid) 
			{ 
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(userLoginVM.UserName,userLoginVM.Password,false,false);
				if (result.Succeeded) {
					return Redirect(userLoginVM.ReturnUrl ?? "/");

				}
				ModelState.AddModelError("", "Invalid username or password");
			}
			return View(userLoginVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel objUser)
		{
			if (ModelState.IsValid) 
			{ 
				AppUserModel newUser = new AppUserModel { UserName = objUser.UserName , Email = objUser.Email};
				IdentityResult result = await _userManager.CreateAsync(newUser, objUser.Password);
				if (result.Succeeded)
				{
					TempData["success"] = "Tạo user thành công";
					return Redirect("/account/login");
				}
				foreach(IdentityError error in result.Errors){

				ModelState.AddModelError("", error.Description);
				}

			}
			return View(objUser);
		}
		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}
	}
}
