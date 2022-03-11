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

        public async Task<IActionResult> All()
        {
            var restaurants = await restaurantService.GetAll();

            return View(restaurants);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantViewModel model, IFormFile file)
        {
            //var filePath = Path.GetTempFileName(); // Full path to file in temp location
            var fileType = "." + file.FileName.Split(".", StringSplitOptions.RemoveEmptyEntries)[1];
            var filePath = Path.Combine("wwwroot", "image",model.Name) + fileType;

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Cant save file");
                return View();
            }

            model.Url = "/Image/Restaurant/" + model.Name + fileType;

            var (created, error) = await restaurantService.Create(model);

            if (!created)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Restaurant/All");
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

        public IActionResult Edit() => View();
    }
}
