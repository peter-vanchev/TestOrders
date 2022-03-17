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
        public async Task<IEnumerable<OrderViewModel>> GetAll()
        {
            var orders = await repo.All<OrderData>()
                .Include(x => x.Order)
                .ThenInclude(r => r.Address)
                .OrderByDescending(d => d.Create)
                .Select(o => new OrderViewModel
                {
                    Id = o.OrderId,
                    Town = o.Order.Address.Town,
                    Aria = o.Order.Address.Area,
                    Street = o.Order.Address.Street + ", " + o.Order.Address.Number,
                    UserName = o.Order.UserName,
                    PhoneNumner = o.Order.PhoneNumner,
                    PaymentType = o.Order.PaymentType,
                    Price = o.Order.Price,
                    DeliveryPrice = o.Order.DeliveryPrice,
                    RestaurantName = o.Order.Restaurant.Name,
                    UserId = o.User.Id,
                    UserCreatedName = o.User.UserName,
                    Status = o.Status,
                    DataCreated = o.Create,
                    LastStatusTime = o.LastUpdate,
                    DriverName = o.Driver.Name
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
                Area = model.Aria,
                Street = model.Street,
                Number = model.Number,
                Other = model.AddressOther
            };

            var user = await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var rest = await repo.All<Restaurant>()
                .Where(x => x.Id == model.RestaurantId)
                .FirstOrDefaultAsync();

            var order = new Order()
            {
                UserName = model.UserName,
                PhoneNumner = model.PhoneNumner,
                PaymentType = model.PaymentType,
                DeliveryPrice = model.DeliveryPrice,
                TimeForDelivery = model.TimeForDelivery,
                Price = model.Price,
                AddressId = address.Id,
                Address = address,
                RestaurantId = rest.Id,
                Restaurant = rest,
                UserCreated = user,
                UserId = userId,
                Description = model.Description,
                Status = model.Status
            };


            var orderData = new OrderData()
            {
                Order = order,
                OrderId = order.Id,
                Create = DateTime.Now,
                Status = Status.Нова,
                ApplicationUserId = userId,
                User = user,
                RestaurantId = rest.Id,
                Restaurant = rest,
                LastUpdate = DateTime.Now,
            };

            try
            {
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
                .Where(x => x.OrderId == Guid.Parse(orderId))
                .Include(x => x.Order)
                .ThenInclude(r => r.Address)
                .Select(o => new OrderViewModel
                {
                    Id = o.OrderId,
                    Town = o.Order.Address.Town,
                    Aria = o.Order.Address.Area,
                    Street = o.Order.Address.Street + ", " + o.Order.Address.Number,
                    UserName = o.Order.UserName,
                    PhoneNumner = o.Order.PhoneNumner,
                    PaymentType = o.Order.PaymentType,
                    Price = o.Order.Price,
                    DeliveryPrice = o.Order.DeliveryPrice,
                    RestaurantName = o.Order.Restaurant.Name,
                    UserId = o.User.Id,
                    UserCreatedName = o.User.UserName,
                    Status = o.Status,
                    DataCreated = o.Create,
                    LastStatusTime = o.LastUpdate,
                    DriverName = o.Driver.Name
                }).FirstOrDefaultAsync();

            return order;
        }

        public async Task<(bool created, string error)> AsignDriver(OrderViewModel model)
        {
            bool created = true;
            string error = null;

            var order = await repo.All<Order>()
                .Where(x => x.Id == model.Id)
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
                .Select(x => new DriverViewModel
                {
                    Id = (Guid)x.DriverId,
                    Email = x.Email,
                    Status = x.Driver.Status
                })
                .ToListAsync();

            return drivers;
        }

    }

}

