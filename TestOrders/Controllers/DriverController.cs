using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class DriverController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDriverServices driverServices;
        private readonly IFileService fileService;

        public DriverController(
            ILogger<HomeController> _logger,
            UserManager<ApplicationUser> _userManager,
            IDriverServices _driverServices,
            IFileService _fileService)
        {
            logger = _logger;
            userManager = _userManager;
            driverServices = _driverServices;
            fileService = _fileService;
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
        public async Task<IActionResult> Create(DriverViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await fileService.SaveFile("Drivers", (model.CarModel + model.CarNumber), file);

            if (!result.saved)
            {
                ModelState.AddModelError("", result.error);
                return View();
            }

            model.CarUrl = "/Image/Drivers/" + result.fileName;
            var (created, error) = await driverServices.Create(model);

            if (!created)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Driver/All");
        }
    }
}
