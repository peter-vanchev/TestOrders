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

        public async Task<IActionResult> All(string from, string to)
        {
            var userId = userManager.GetUserId(User);
            var drivers = await driverServices.GetAll();
            ViewBag.drivers = drivers;

            if (string.IsNullOrEmpty(from))
            {
                var orders = await orderService.GetAll(userId);

                return this.View(orders.ToList());
            }
            else if (string.IsNullOrEmpty(to))
            {
                var orders = await orderService.GetAll(userId, DateTime.Parse(from), DateTime.Now);

                return this.View(orders.ToList());
            }
            else
            {
                var toDate = DateTime.Parse(to).AddDays(1);
                var orders = await orderService.GetAll(userId, DateTime.Parse(from), toDate);

                return this.View(orders.ToList());
            }
        }

        [Authorize(Roles = "Admin, Manager, Driver")]
        public async Task<IActionResult> Action(string id, bool accepted)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await orderService.AcceptOrder(userId, id, accepted);

            if (result.Item1)
            {
                return Redirect("/Order/All");
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
            ViewData["restaurants"] = await FindRestaurants();

            return View();
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            ViewData["restaurants"] = await FindRestaurants();

            if (!ModelState.IsValid )
            {                   
                return View();
            }

            var (created, error) = await orderService.CreateAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            if (!created)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Order/All");
        }

        public async Task<IActionResult> Details(string Id)
        {
            var order = await orderService.GetOrdersDetails(Id);
            return View(order);
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        public async Task<IActionResult> Edit(string Id)
        {
            ViewData["restaurants"] = await FindRestaurants();

            var order = await orderService.GetOrderById(Id);
            return View(order);
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            ViewData["restaurants"] = await FindRestaurants();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var (edited, error) = await orderService.EditAsync(model);

            if (!edited)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Order/All");
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

        private async Task<List<RestaurantViewModel>> FindRestaurants()
        {
            var userId = userManager.GetUserId(User);

            if (this.User.IsInRole("Admin"))
            {
                var restaurants = await restaurantService.GetAllAsync();
                return restaurants.ToList();
            }
            else
            {
                var user = await userManager.FindByIdAsync(userId);
                var restaurant = await restaurantService.GetRestaurantById(user.RestaurantId.ToString());
                var restaurants = new List<RestaurantViewModel>();
                restaurants.Add(restaurant);
                return restaurants;
            }
        }
    }
}
