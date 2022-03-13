using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestOrders.Contracts;
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;


namespace TestOrders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public OrderService(
            IRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        [Authorize]
        public async Task<IEnumerable<OrderViewModel>> GetAll(string userId)
        {
            var user = userManager.Users.Where(x => x.Id == userId.ToString()).FirstOrDefault();

                var orders = await repo.All<OrderData>()
                    .Include(x => x.Order)
                    .ThenInclude(r => r.Address)
                    .OrderByDescending(d => d.Create)
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
                        LastStatusTime = o.Create
                    }).ToListAsync();
            
            return orders;
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
                RestaurantId = (Guid)user.RestaurantId,
                Restaurant = rest,
                User = user,
                UserId = userId
            };


            var orderData = new OrderData()
            {
                Order = order,
                OrderId = order.Id,
                Create = DateTime.Now,
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
            var order = await repo.All<OrderData>()
                .Where(x => x.Id == Guid.Parse(orderId))
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
                    RestaurantId = (Guid)o.Order.RestaurantId,
                    RestaurantName = o.Order.Restaurant.Name,
                    UserId = o.Order.UserId.ToString(),
                    UserName = o.Order.User.UserName,
                    Status = o.Status,
                    LastStatusTime = o.Create,
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
                .Where(x => x.Id == model.DriverId)
                .FirstOrDefaultAsync();

            order.PaymentType = model.PaymentType;
            order.Price = model.Price;
            order.DeliveryPrice = model.DeliveryPrice;
            order.RestaurantId = model.RestaurantId;
            order.Restaurant = restaurant;
            order.PhoneNumner = model.PhoneNumner;
            order.Status = Status.Изпратена;


            var orderStatus = new OrderData
            {
                Create = DateTime.Now,
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

        public async Task<IEnumerable<DriverViewModel>> GetFreeDrivers()
        {
            var drivers = await repo.All<ApplicationUser>()
                .Include(u => u.Driver)                
                .Where(x => x.Driver.Status == Status.Свободен)                               
                .Select(x => new DriverViewModel {
                    Id = (Guid)x.DriverId,
                    Email = x.Email,
                    Status = x.Driver.Status
                })
                .ToListAsync();

            return drivers;
        }

    }

}

