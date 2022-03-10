using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestOrders.Contracts;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            logger = _logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return Redirect("/Admin");
            }
            else if(User.IsInRole("Restaurant"))
            {
                return Redirect("/Restaurant");
            }

            return Redirect("Restaurant/All");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}