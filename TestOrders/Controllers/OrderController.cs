using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
using System.Security.Claims;

namespace TestOrders.Controllers
{
    public class OrderController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly IRestaurantService restaurantService;
        private readonly IDriverServices driverServices;

        public OrderController(
            IOrderService _orderService,
            UserManager<ApplicationUser> _userManager,
            IRestaurantService _restaurantService,
            IDriverServices _driverServices)
        {
            orderService = _orderService;
            userManager = _userManager;
            restaurantService = _restaurantService;
            driverServices = _driverServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> All(string sortOrder)
        {
            var drivers = await driverServices.GetAll();
            ViewBag.drivers = drivers;

            var orders = await orderService.GetAll(userManager.GetUserId(User));


            ViewData["RestName"] = sortOrder == "restName" ? "restName_desc" : "restName";
            ViewData["UserName"] = sortOrder == "userName" ? "userName_desc" : "userName";
            ViewData["DriverName"] = sortOrder == "driverName" ? "driverName_desc" : "driverName";

            switch (sortOrder)
            {
                case "restName":
                    orders = orders.OrderBy(s => s.RestaurantName);
                    break;
                case "restName_desc":
                    orders = orders.OrderByDescending(s => s.RestaurantName);
                    break;
                case "userName":
                    orders = orders.OrderBy(s => s.UserName);
                    break;
                case "userName_desc":
                    orders = orders.OrderByDescending(s => s.UserName);
                    break;
                case "driverName":
                    orders = orders.OrderBy(s => s.DriverName);
                    break;
                case "driverName_desc":
                    orders = orders.OrderByDescending(s => s.DriverName);
                    break;
                default:
                    break;
            }

            return this.View(orders.ToList());
        }

        [Authorize(Roles = "Admin, Manager, Driver")]
        public async Task<IActionResult> Action(string id, bool accepted)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await orderService.AcceptOrder(userId, id, accepted);

            if (result.Item1)
            {
                return Redirect("/Order/NewOrders");
            }

            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> AsignOrder(Guid driverId, Guid orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (asign, error) = await driverServices.AsignDriver(driverId, orderId, userId);

            if (!asign)
            {
                return View(error, "/Error");
            }

            return Redirect("/Order/All");
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        public async Task<IActionResult> Create()
        {
            if (this.User.IsInRole("Admin"))
            {
                var restaurants = await restaurantService.GetAll();
                ViewData["restaurants"] = restaurants.ToList();
            }
            else
            {
                var userId = userManager.GetUserId(User);
                var user = await userManager.FindByIdAsync(userId);
                var restaurant = await restaurantService.GetRestaurantById(user.RestaurantId.ToString());
                var restaurants = new List<RestaurantViewModel>();
                restaurants.Add(restaurant);
                ViewData["restaurants"] = restaurants;
            }

            return View();
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid || model.PaymentType == "false")
            {
                if (this.User.IsInRole("Admin"))
                {
                    var restaurants = await restaurantService.GetAll();
                    ViewData["restaurants"] = restaurants.ToList();
                }
                else
                {
                    var user = await userManager.FindByIdAsync(userId);
                    var restaurant = await restaurantService.GetRestaurantById(user.RestaurantId.ToString());
                    var restaurants = new List<RestaurantViewModel>();
                    restaurants.Add(restaurant);
                    ViewData["restaurants"] = restaurants;
                }

                return View();
            }

            var (created, error) = await orderService.Create(model, userId);
            if (!created)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Order/NewOrders");
        }

        public async Task<IActionResult> Details(string Id)
        {
            var order = await orderService.GetOrderById(Id);
            return View(order);
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        public async Task<IActionResult> Edit(string Id)
        {
            var order = await orderService.GetOrderById(Id);
            return View(order);
        }

        [Authorize(Roles = "Admin, Manager, Driver")]
        public async Task<IActionResult> Delivery(Guid orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (asign, error) = await orderService.DeliveryOrder(orderId, userId);

            if (!asign)
            {
                return View(error, "/Error");
            }

            return Redirect("/Order/All");
        }

        public async Task<IActionResult> NewOrders()
        {
            var orders = await orderService.GetAll(userManager.GetUserId(User));

            if (this.User.IsInRole("Driver"))
            {
                orders = orders.Where(
                 x => x.Status == Status.Насочена)
                 .ToList();
                return this.View(orders);
            }

            orders = orders.Where(
                x => x.Status == Status.Нова)
                .ToList();
            return this.View(orders);
        }
    }
}
