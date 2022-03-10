using Microsoft.AspNetCore.Mvc;
using TestOrders.Contracts;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
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
