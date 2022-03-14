using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class RestaurantController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IFileService fileService;
        private readonly IRestaurantService restaurantService;

        public RestaurantController(
            IFileService _fileService,
            IOrderService _orderService,
            IRestaurantService _restaurantService)
        {
            orderService = _orderService;
            fileService = _fileService;
            restaurantService = _restaurantService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> All()
        {
            var restaurants = await restaurantService.GetAll();

            return View(restaurants);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await fileService.SaveFile("Restaurants", model.Name, file);

            if (!result.saved)
            {
                ModelState.AddModelError("", result.error);
                return View();
            }

            model.Url = "/Image/Restaurants/" + result.fileName;

            var (created, error) = await restaurantService.Create(model);

            if (!created)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Restaurant/All");
        }

        public async Task<IActionResult> Details(string Id)
        {
            var restaurant = await restaurantService.GetRestaurantById(Id);
            return View(restaurant);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var restaurant = await restaurantService.GetRestaurantById(Id);
            return View(restaurant);
        }


        public IActionResult RemoveRestaurant(string restaurantId)
        {
            var (delete, error) = restaurantService.Delete(restaurantId);

            if (!delete)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Restaurant");
        }        
    }
}
