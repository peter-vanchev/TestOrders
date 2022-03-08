﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using TestOrders.Contracts;
=======
using System.Globalization;
using TestOrders.Contracts;
using TestOrders.Data;
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;


namespace TestOrders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;
<<<<<<< HEAD

        public OrderService(
            IRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
=======
        private readonly ApplicationDbContext dbContext;

        public OrderService(
            IRepository _repo,
            UserManager<ApplicationUser> _userManager,
            ApplicationDbContext _dbContext)
        {
            repo = _repo;
            userManager = _userManager;
            dbContext = _dbContext;
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
        }

        [Authorize]
        public async Task<IEnumerable<OrderViewModel>> GetAll(string userId)
        {
            var user = userManager.Users.Where(x => x.Id == userId).FirstOrDefault();

                var orders = await repo.All<OrderStatus>()
                    .Include(x => x.Order)
                    .ThenInclude(r => r.Address)
                    .OrderByDescending(d => d.Time)
                    .Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        Town = o.Order.Address.Town,
                        Street = o.Order.Address.Street,
                        Number = o.Order.Address.Number,
                        Email = o.Order.Email,
                        PhoneNumner = o.Order.PhoneNumner,
                        PaymentType = o.Order.PaymentType,
                        Price = o.Order.Price,
                        DeliveryPrice = o.Order.DeliveryPrice,
                        RestaurantName = o.Order.Restaurant.Name,
                        UserId = userId,
                        UserName = user.UserName,
                        Status = o.Status,
                        LastStatusTime = o.Time
                    }).ToListAsync();
<<<<<<< HEAD
            
            return orders;
=======

                return orders;
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
        }

        public async Task<(bool created, string error)> Create(OrderViewModel model, string userId)
        {
            bool created = true;
            string error = null;

            var address = new Address()
            {
                Town = "София",
                Number = model.Number,
                Street = model.Street
            };

            var user = await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var rest = await repo.All<Restaurant>()
                .Where(x => x.Id == user.RestaurantId)
                .FirstOrDefaultAsync();

            var order = new Order()
            {
                Email = model.Email,
                PhoneNumner = model.PhoneNumner,
                PaymentType = model.PaymentType,
                DeliveryPrice = model.DeliveryPrice,
                Price = model.Price,
                AddressId = address.Id,
                Address = address,
                RestaurantId = user.RestaurantId,
                Restaurant = rest,
                User = user,
                UserId = userId
            };

            var orderData = new OrderStatus()
            {
                Order = order,
                OrderId = order.Id,
                Time = DateTime.Now,
                Status = Status.Нова
            };

            try
            {
                repo.Add(order);
                repo.Add(orderData);

                repo.SaveChanges();
                created = true;
            }

            catch (Exception)
            {
                error = "Could not save Order";
            }

            return (created, error);
        }

        public async Task<OrderViewModel> GetOrderById(string orderId)
        {
            var order = await repo.All<OrderStatus>()
                .Where(x => x.Id == orderId)
                .Include(x => x.Order)
                .ThenInclude(r => r.Address)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Town = o.Order.Address.Town,
                    Street = o.Order.Address.Street,
                    Number = o.Order.Address.Number,
                    Email = o.Order.Email,
                    PhoneNumner = o.Order.PhoneNumner,
                    PaymentType = o.Order.PaymentType,
                    Price = o.Order.Price,
                    DeliveryPrice = o.Order.DeliveryPrice,
                    RestaurantId = o.Order.RestaurantId,
                    RestaurantName = o.Order.Restaurant.Name,
                    UserId = o.Order.UserId,
                    UserName = o.Order.User.UserName,
                    Status = o.Status,
                    LastStatusTime = o.Time,
                    Order = o.Order,
                    OrderId = o.OrderId
                }).FirstOrDefaultAsync();

          return order;
        }

        public async Task<(bool created, string error)> AsignDriver(OrderViewModel model)
        {
            bool created = true;
            string error = null;

            var order = await repo.All<Order>()
                .Where(x => x.Id == model.OrderId)
                .FirstOrDefaultAsync();

            var restaurant = await repo.All<Restaurant>()
                .Where(x => x.Id == model.RestaurantId)
                .FirstOrDefaultAsync();

            var driver = await repo.All<Driver>()
                .Where(x => x.UserId == model.DriverId)
                .FirstOrDefaultAsync();

            order.PaymentType = model.PaymentType;
            order.Price = model.Price;
            order.DeliveryPrice = model.DeliveryPrice;
            order.RestaurantId = model.RestaurantId;
            order.Restaurant = restaurant;
            order.PhoneNumner = model.PhoneNumner;
            order.Status = Status.Изпратена;

            var orderStatus = new OrderStatus
            {
                Time = DateTime.Now,
                DriverId = model.DriverId,
                Driver = driver,
                Order = order,
                OrderId = model.Id,            
                Status = Status.Изпратена
            };

            try
            {
                repo.Update(order);
                repo.Add(orderStatus);

                repo.SaveChanges();
                created = true;
            }

            catch (Exception)
            {
                error = "Could not save Order";
            }

            return (created, error);
        }

        public async Task<IEnumerable<UserRolesViewModel>> GetFreeDrivers()
        {
            var drivers = await repo.All<Driver>()
                .Where(x => x.Status == Status.Свободен)                               
                .Select(x => new UserRolesViewModel { 
                    Email = x.User.Email,
                    UserName = x.User.UserName,
                    UserId = x.User.Id,
                    Roles = new List<string> { x.User.Id }
                })
                .ToListAsync();

            return drivers;
        }

    }

}

