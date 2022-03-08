using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRestaurantService restaurantService;


        public RestaurantController(
            UserManager<ApplicationUser> _userManager, 
            IOrderService _orderService,
            IRestaurantService _restaurantService)
        {
            orderService = _orderService;               
            userManager = _userManager;
            restaurantService = _restaurantService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult All()
        {
            var restaurants = restaurantService.GetAll();
            return View(restaurants);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(RestaurantViewModel model)
        {
            var (created, error) = restaurantService.Create(model);
            if (!created)
            {
                return View(error, "/Error");
            }

            return Redirect("/Restaurant");
        }

        public IActionResult RemoveRestaurant(string restaurantId)
        {
            var (delete, error) = restaurantService.Delete(restaurantId);

            if (!delete)
            {
                return View(error, "/Error");
            }

            return Redirect("/Restaurant");
        }

        public IActionResult Edit() => View();
    }
}
