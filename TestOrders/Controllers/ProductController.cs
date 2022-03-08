using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IProductService productService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductController(
            ILogger<HomeController> _logger,
            IProductService _productService,
            UserManager<ApplicationUser> _userManager)
        {
            productService = _productService;
            logger = _logger;
            userManager = _userManager;
        }

        public IActionResult Index(string Id)
        {
            var products = productService.GetAll(Id);
            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            var (created, error) = productService.Create(model);
            if (!created)
            {
                return View(error, "/Error");
            }

            return Redirect("/Product");
        }


    }
}
