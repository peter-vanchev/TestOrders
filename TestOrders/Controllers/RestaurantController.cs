using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orders.Core.Contracts;
using Orders.Core.Models;

namespace TestOrders.Controllers
{
    public class RestaurantController : BaseController
    {
        private readonly IFileService fileService;
        private readonly IRestaurantService restaurantService;

        public RestaurantController(
            IFileService _fileService,
            IRestaurantService _restaurantService)
        {
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin, Restaurant")]
        public async Task<IActionResult> Edit(string Id)
        {
            var restaurant = await restaurantService.GetRestaurantById(Id);
            var restEdit = JsonConvert.DeserializeObject<EditRestaurantViewModel>(JsonConvert.SerializeObject(restaurant));          

            return View(restEdit);
        }

        [Authorize(Roles = "Admin, Restaurant")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditRestaurantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (edited, error) = await restaurantService.Edit(model);

            if (!edited)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Restaurant/All");
        }
    }
}
