using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
using System.Globalization;
using System.Security.Claims;

namespace TestOrders.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IOrderService orderService;

        public AdminController(
            IAdminService _adminService,
            IOrderService _orderService)
        {
            adminService = _adminService;
            orderService = _orderService;
        }

        public async Task<IActionResult> Index(string period = "week")
        {
            //var result = await adminService.Seed();

            //if (result)
            //{
            //    SignOut();
            //}

            var (startDate, endDate) = CheckPeriod(period);            

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);            

            var orders = await orderService.GetStats(userId, startDate, endDate);

            return View(orders);
        }


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

        private (DateTime, DateTime) CheckPeriod(string period)
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Now;

            switch (period)
            {
                case "year":
                    startDate = new DateTime(endDate.Year, 1, 1);
                    break;
                case "month":
                    startDate = new DateTime(endDate.Year, endDate.Month, 1);
                    break;
                case "week":
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                    break;
                default:
                    break;
            }

            return (startDate, endDate);
        }
    }
}
