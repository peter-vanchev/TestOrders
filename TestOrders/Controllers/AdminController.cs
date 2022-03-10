using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Models;

namespace TestOrders.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService _adminService)
        {               
            adminService = _adminService;
        }

        public IActionResult Index() => View();
       

        public async Task<IActionResult> Role()
        {
            var roles = await adminService.GetRole();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var suxess = await adminService.AddRole(roleName);

            return RedirectToAction("Role");
        }

        public async Task<IActionResult> UserRole()
        {
            var userRoles = await adminService.GetUserRoles();

            return View(userRoles);
        }

        public async Task<IActionResult> ManageRole(string userId)
        {
            ViewBag.userId = userId;
            var user = await adminService.AddRoleToUser(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRole(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await adminService.ManageRole(model, userId);

            if (!user.Item1)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View();
            }

            return RedirectToAction("UserRole");
        }
    }
}
