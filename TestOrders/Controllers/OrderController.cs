﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TestOrders.Contracts;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Controllers
{
    public class OrderController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IOrderService orderService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService adminService;
        private readonly IRestaurantService restaurantService;

        public OrderController(
            ILogger<HomeController> _logger,
            IOrderService _orderService,
            UserManager<ApplicationUser> _userManager,
            IAdminService _adminService,
            IRestaurantService _restaurantService)
        {
            orderService = _orderService;
            logger = _logger;
            userManager = _userManager;
            adminService = _adminService;
            restaurantService = _restaurantService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> All()
        {
            var orders = await orderService.GetAll(userManager.GetUserId(User));

            return this.View(orders);
        }


        public async Task<IActionResult> Create()
        {
            var test = await restaurantService.GetAll();
            ViewData["restaurants"] = test.ToList();
            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                var test = await restaurantService.GetAll();
                ViewData["restaurants"] = test.ToList();
                return View();
            }

            var userId = userManager.GetUserId(User);
            
            var (created, error) = await orderService.Create(model, userId);
            if (!created)
            {
                return View(error, "/Error");
            }

            return Redirect("/Order/All");
        }

        public async Task<IActionResult> Details(string Id)
        {
            var order = await orderService.GetOrderById(Id);
            return View(order);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            ViewBag.drivers = await orderService.GetFreeDrivers();
            var order = await orderService.GetOrderById(Id);
            var restorants = await restaurantService.GetAll();
            ViewBag.restorants = restorants;
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            var (asign, error) = await orderService.AsignDriver(model);

            if (!asign)
            {
                return View(error, "/Error");
            }

            return Redirect("/Order/All");
        }
    }
}
