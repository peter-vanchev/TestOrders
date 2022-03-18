using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class OrderController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IOrderService orderService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService adminService;
        private readonly IRestaurantService restaurantService;

        public OrderController(
            ILogger<HomeController> _logger,
            IOrderService _orderService,
            UserManager<ApplicationUser> _userManager,
            IAdminService _adminService,
            IRestaurantService _restaurantService)
        {
            orderService = _orderService;
            logger = _logger;
            userManager = _userManager;
            adminService = _adminService;
            restaurantService = _restaurantService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> All()
        {
            var orders = await orderService.GetAll();

            return this.View(orders.ToList());
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
            var orders = await orderService.GetAll();

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
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();
               
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
                return View(error, "/Error");
            }

            return Redirect("/Order/All");
        }

        public async Task<IActionResult> Details(string Id)
        {
            var order = await orderService.GetOrderById(Id);
            return View(order);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            ViewBag.drivers = await orderService.GetFreeDrivers();
            var order = await orderService.GetOrderById(Id);
            var restorants = await restaurantService.GetAll();
            ViewBag.restorants = restorants;
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            var (asign, error) = await orderService.AsignDriver(model);

            if (!asign)
            {
                return View(error, "/Error");
            }

            return Redirect("/Order/All");
        }
    }
}
