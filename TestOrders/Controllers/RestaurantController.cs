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

<<<<<<< HEAD
        public async Task<IActionResult> All()
        {
            //var user = userManager.Users.Where(x => x.Email == "Stroeja@abv.bg").FirstOrDefault();

            //var temp = await userManager.ChangePasswordAsync(user, "Stroeja1", "Ala-Bala-123");
            //var temp1 = await userManager.AddPasswordAsync(user, "Ala-Bala-123");

            var restaurants = restaurantService.GetAll();

            //if (temp.Succeeded)
            //{
            //    return View(restaurants);

            //}
=======
        public IActionResult All()
        {
            var restaurants = restaurantService.GetAll();
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
            return View(restaurants);
        }

        public IActionResult Create() => View();

        [HttpPost]
<<<<<<< HEAD
        public async Task<IActionResult> Create(RestaurantViewModel model)
        {
            var (created, error) = await restaurantService.Create(model);
=======
        public IActionResult Create(RestaurantViewModel model)
        {
            var (created, error) = restaurantService.Create(model);
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
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
