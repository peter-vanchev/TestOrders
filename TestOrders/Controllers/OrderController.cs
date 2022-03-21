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
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly IAdminService adminService;
        private readonly IRestaurantService restaurantService;
        private readonly IDriverServices driverServices;

        public OrderController(
            ILogger<HomeController> _logger,
            IOrderService _orderService,
            UserManager<ApplicationUser> _userManager,
            IAdminService _adminService,
            IRestaurantService _restaurantService,
            IDriverServices _driverServices)
        {
            orderService = _orderService;
            logger = _logger;
            userManager = _userManager;
            adminService = _adminService;
            restaurantService = _restaurantService;
            driverServices = _driverServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> All()
        {
            var drivers = await driverServices.GetFreeDrivers();
            ViewBag.drivers = drivers;

            var orders = await orderService.GetAll(userManager.GetUserId(User));
            return this.View(orders.Where(x => x.Status != Status.Нова).ToList());
        }

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

        public async Task<IActionResult> NewOrders()
        {
            var orders = await orderService.GetAll(userManager.GetUserId(User));

            return this.View(orders.Where(x => x.Status == Status.Нова).ToList());
        }

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

        public async Task<IActionResult> Edit(string Id)
        {
            var order = await orderService.GetOrderById(Id);
            return View(order);
        }

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
    }
}
