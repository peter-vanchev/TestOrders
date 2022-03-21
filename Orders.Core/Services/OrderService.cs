using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
using Orders.Infrastructure.Data.Repositories;

namespace Orders.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IApplicatioDbRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public OrderService(
            IApplicatioDbRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAll()
        {
            var order1 = await repo.All<Order>()
              .Include(x => x.OrderDatas)              
              .Select(o => new OrderViewModel
              {
                  Id = o.Id,
                  Town = o.Address.Town,
                  Aria = o.Address.Area,
                  Street = o.Address.Street,
                  Number = o.Address.Number,
                  UserName = o.UserName,
                  PhoneNumner = o.PhoneNumner,
                  PaymentType = o.PaymentType,
                  Price = o.Price,
                  DeliveryPrice = o.DeliveryPrice,
                  TimeForDelivery = o.TimeForDelivery,
                  RestaurantId = o.RestaurantId,
                  RestaurantName = o.Restaurant.Name,
                  Status = o.Status,
                  DataCreated = o.OrderDatas.Max(x => x.LastUpdate),
                  DriverName = String.Join(" ", o.Driver.User.FirstName, o.Driver.User.LastName)
              })
              .OrderByDescending(x => x.DataCreated)
              .ToListAsync();

            return order1;
        }

        public async Task<(bool created, string error)> Create(OrderViewModel model, string userId)
        {
            bool created = true;
            string error = "";

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
                UserId = userId,
                UserCreated = user,
                Description = model.Description,
                Create = DateTime.Now,
                Status = Status.Нова
            };


            var orderData = new OrderData()
            {
                Order = order,
                OrderId = order.Id,
                Status = Status.Нова,
                LastUpdate = DateTime.Now,
                User = user,
                UserId = user.Id
            };

            try
            {
                await repo.AddAsync(orderData);
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
                    Street = o.Order.Address.Street,
                    Number = o.Order.Address.Number,
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
                    DriverName = String.Join(" ", o.Driver.User.FirstName, o.Driver.User.FirstName)
                }).FirstOrDefaultAsync();

            return order;
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

            var orderData = new OrderData()
            {
                OrderId = Guid.Parse(orderId),
                Order = order,
                LastUpdate = DateTime.Now,
                Status = order.Status,
                UserId = userId,
                User = user
            };

            action = false;
            try
            {
                repo.Update(order);
                await repo.AddAsync(orderData);

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

