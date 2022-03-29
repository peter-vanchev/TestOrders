using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
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

        public async Task<IActionResult> Index()
        {
            //var result = await adminService .Seed();

            //if (result)
            //{
            //    SignOut();
            //}
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await orderService.GetAll(userId);
            var totalSells = orders
                .Select(x => x.DeliveryPrice).Sum();

            ViewBag.Orders = orders.Count();
            ViewBag.NewOrders = orders.Where(x => x.Status == Status.Нова).Count();
            ViewBag.EndOrders = orders.Where(x => x.Status == Status.Доставена).Count();
            ViewBag.TotalSells = totalSells;
            ViewBag.Proogres = (orders.Where(x => x.Status == Status.Доставена).Count() / (double)orders.Count()) * 100;
            return View();
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
    }
}
