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
                .OrderByDescending(d => d.Order.Create)
                .Select(o => new OrderViewModel
                {
                    Id = o.OrderId,
                    Town = o.Order.Address.Town,
                    Aria = o.Order.Address.Area,
                    Street = o.Order.Address.Street + ", " + o.Order.Address.Number,
                    UserId = o.Order.UserCreated.Id,
                    UserName = o.Order.UserName,
                    PhoneNumner = o.Order.PhoneNumner,
                    PaymentType = o.Order.PaymentType,
                    Price = o.Order.Price,
                    DeliveryPrice = o.Order.DeliveryPrice,
                    RestaurantName = o.Order.Restaurant.Name,
                    Status = o.Status,
                    DataCreated = o.Order.Create,
                    LastStatusTime = o.LastUpdate,
                    DriverName = o.Driver.Name
                }).ToListAsync();

            var order1 = await repo.All<Order>()
              .Include(x => x.OrderData)
              .Select(o => new OrderViewModel
              {
                  Id = o.Id,
                  Town = o.Address.Town,
                  Aria = o.Address.Area,
                  Street = o.Address.Street + ", " + o.Address.Number,
                  UserName = o.UserName,
                  PhoneNumner = o.PhoneNumner,
                  PaymentType = o.PaymentType,
                  Price = o.Price,
                  DeliveryPrice = o.DeliveryPrice,
                  RestaurantName = o.Restaurant.Name,
                  Status = o.Status,
                  DataCreated = o.Create,
                  DriverName = o.Driver.Name
               })
              .ToListAsync();


            return order1;
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
                Create = DateTime.Now,
                Status = model.Status
            };


            var orderData = new OrderData()
            {
                Order = order,
                OrderId = order.Id,
                Status = Status.Нова,
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
                    UserId = o.Order.UserCreated.Id,
                    UserCreatedName = o.Order.UserName,
                    Status = o.Status,
                    DataCreated = o.Order.Create,
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

        public async Task<(bool, string)> AcceptOrder(string userId, string orderId, bool action) 
        {
            var error = "";
            var order = await repo.All<Order>()
                .Where(x => x.Id == Guid.Parse(orderId))
                .FirstOrDefaultAsync();

            var user = await repo.All<ApplicationUser>()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (!action)
            {
                order.Status = Status.Отказана;
            }
            else
            {
                order.Status = Status.Приета;
            }

            var orderData = new OrderData() {
                OrderId = Guid.Parse(orderId),
                Order = order,
                LastUpdate = DateTime.Now,
                Status = order.Status,
            };

            action = false;
            try
            {
                repo.Update(order);
                repo.Add(orderData);

                repo.SaveChanges();
                action = true;
            }

            catch (Exception)
            {
                error = "Could not save Order";
            }

            return (action, error);
        }
    }

}

