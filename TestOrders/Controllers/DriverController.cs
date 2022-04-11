using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;

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
            var drivers = await driverServices.GetAllAsyncl();

            return View(drivers);
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(DriverViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await fileService.SaveFile("Drivers", (model.LastName + model.CarNumber), file);

            if (!result.saved)
            {
                ModelState.AddModelError("", result.error);
                return View();
            }

            model.DriverUrl = "/Image/Drivers/" + result.fileName;
            var (created, error) = await driverServices.CreateAsync(model);

            if (!created)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Driver/All");
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            var driver = await driverServices.GetDriverByIdAsync(Id);
            return View(driver);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var driver = await driverServices.GetDriverByIdAsync(Id);
            return View(driver);
        }

        [Authorize(Roles = "Admin, Manager, Restaurant")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditDriverViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (edited, error) = await driverServices.EditAsync(model);

            if (!edited)
            {
                ModelState.AddModelError("", error);
                return View();
            }

            return Redirect("/Driver/All");
        }
    }
}
