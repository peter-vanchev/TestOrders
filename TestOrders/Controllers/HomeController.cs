using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace TestOrders.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IOrderService orderService;


        public HomeController(
            ILogger<HomeController> _logger,
            IOrderService _orderService)
        {
            logger = _logger;
            orderService = _orderService;
        }

        public async Task<IActionResult> Index(string period = null)
        {

            var (startDate, endDate) = CheckPeriod(period);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await orderService.GetStats(userId, startDate, endDate);

            return View(orders);
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

        private (DateTime, DateTime) CheckPeriod(string period)
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Now;

            switch (period)
            {
                case "year":
                    startDate = new DateTime(endDate.Year, 1, 1);
                    break;
                case "month":
                    startDate = new DateTime(endDate.Year, endDate.Month, 1);
                    break;
                case "week":
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                    break;
                default:
                    break;
            }

            return (startDate, endDate);
        }

    }
}