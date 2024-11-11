using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;
using shoppingcart.Repositories;

namespace shoppingcart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Role")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly DataContext _dataContext;
		private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = context;
			_roleManager = roleManager;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Roles.OrderByDescending(p => p.Id).ToListAsync());
        }
		[Route("Create")]
		public IActionResult Create()
		{
			return View("Create");
		}
		[Route("Create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(IdentityRole role)
		{
            if (!_roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(role.Name)).GetAwaiter().GetResult();
                TempData["success"] = "Create new role successfully";
                return Redirect("Index");
            }
            TempData["error"] = "Error or already have it !!!";
            return Redirect("Index");
		}
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }
            var deleteResult = await _roleManager.DeleteAsync(role);
            if (!deleteResult.Succeeded)
            {

                return View("Error");
            }
            TempData["success"] = "Role is successfully deleted";
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string Id, IdentityRole role)
        {
            var existingRole = await _roleManager.FindByIdAsync(Id);
            if (existingRole == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                existingRole.Name = role.Name;
                //existingRole.NormalizedName = role.Name.ToUpper().Replace(" ", "");
                var updateRoleResult = await _roleManager.UpdateAsync(existingRole);
                if (updateRoleResult.Succeeded)
                {
                    TempData["success"] = "Update role successfully";
                    return Redirect("Index");
                }
                else
                {
                    AddIdentityErrors(updateRoleResult);
                    return View(updateRoleResult);
                }
            }
            //Model validation failed 
            TempData["error"] = "Model validation failed.";
            var errors = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToList();
            string errorMessage = string.Join("\n", errors);
            return View(role);
        }
        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        }
    }
}
