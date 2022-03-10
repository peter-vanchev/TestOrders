using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class DriverController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDriverServices driverServices;


        public DriverController(
            ILogger<HomeController> _logger,
            UserManager<ApplicationUser> _userManager,
            IDriverServices _driverServices)
        {
            logger = _logger;
            userManager = _userManager;
            driverServices = _driverServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> All()
        {
            var drivers = await driverServices.GetAll();

            return View(drivers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(DriverViewModel model)
        {
            var (created, error) = await driverServices.Create(model);

            if (!created)
            {
                return View(error, "/Error");
            }

            return Redirect("/Driver/All");
        }
    }
}
