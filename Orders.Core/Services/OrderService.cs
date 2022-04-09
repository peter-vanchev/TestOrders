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

        public async Task<(bool, string)> AcceptOrder(string userId, string orderId, bool action)
        {
            var error = "";
            var actionResult = false;
            var order = await repo.All<Order>()
                .Where(x => x.Id == Guid.Parse(orderId))
                .FirstOrDefaultAsync();

            var user = await repo.All<ApplicationUser>()
                .Include(x => x.Driver)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();
            var userRole = await userManager.GetRolesAsync(user);
            if (order != null)
            {
                if (!action)
                {
                    if (userRole.Contains("Driver"))
                    {
                        order.Status = Status.ОтказанаШофьор;
                    }
                    else
                    {
                        order.Status = Status.Отказана;
                    }
                }
                else
                {
                    if (userRole.Contains("Admin"))
                    {
                        order.Status = Status.Приета;
                    }
                    else
                    {
                        order.Status = Status.Изпратена;
                        user.Driver.Status = Status.Зает;
                    }
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

                actionResult = false;

                try
                {
                    await repo.AddAsync(orderData);

                    repo.SaveChanges();
                    actionResult = true;
                }

                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }

            return (actionResult, error);
        }

        public async Task<(bool created, string error)> CreateAsync(OrderViewModel model, string userId)
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
                PhoneNumner = "+359 / " + model.PhoneNumner,
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

        public async Task<(bool edited, string error)> EditAsync(OrderViewModel model, string userId)
        {
            bool edited = true;
            string error = "";

            var order = await repo.All<Order>()
                .Include(x => x.Address)
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            order.UserName = model.UserName;
            order.Address.Street = model.Street;
            order.Address.Number = model.Number;
            order.Description = model.Description;
            order.PhoneNumner = model.PhoneNumner;
            order.Price = model.Price;
            order.DeliveryPrice = model.DeliveryPrice;
            order.TimeForDelivery = model.TimeForDelivery;
            order.PaymentType = model.PaymentType;
            order.RestaurantId = (Guid)model.RestaurantId;
            order.Create = DateTime.Now;


            var orderData = new OrderData()
            {
                Order = order,
                OrderId = order.Id,
                LastUpdate = DateTime.Now,
                Status = order.Status,
                UserId = userId
            };

            try
            {
                repo.AddAsync(orderData);
                repo.SaveChanges();
                edited = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw;
            }

            return (edited, error);
        }

        public async Task<IEnumerable<OrderViewModel>> GetAll(DateTime? startDate = null, DateTime? endDate = null)
        {
            (startDate, endDate) = CheckDate(startDate, endDate);

            return await repo.All<Order>()
              .Include(x => x.OrderDatas)
              .Select(o => new OrderViewModel
              {
                  Id = o.Id,
                  OrderNumber = o.Number,
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
              .Where(x => x.DataCreated >= startDate && x.DataCreated <= endDate)
              .OrderByDescending(x => x.DataCreated)
              .ToListAsync();
        }

        public async Task<IEnumerable<OrderViewModel>> GetAll(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            (startDate, endDate) = CheckDate(startDate, endDate);

            var user = await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                var orders = await GetAll(startDate, endDate);
                return orders;
            }

            else if (await userManager.IsInRoleAsync(user, "Restaurant"))
            {
                var restaurantId = user.RestaurantId;
                return await repo.All<Order>()
                    .Where(x => x.RestaurantId == restaurantId)
                    .Include(x => x.OrderDatas)
                    .Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        OrderNumber = o.Number,
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
                    .Where(x => x.DataCreated >= startDate && x.DataCreated <= endDate)
                    .OrderByDescending(x => x.DataCreated)
                    .ToListAsync();
            }

            var driverId = user.DriverId;
            return await repo.All<Order>()
                .Where(x => x.DriverId == driverId)
                .Include(x => x.OrderDatas)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.Number,
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
                .Where(x => x.DataCreated >= startDate && x.DataCreated <= endDate)
                .OrderByDescending(x => x.DataCreated)
                .ToListAsync();
        }

        public async Task<OrderViewModel> GetOrderById(string orderId)
        {

            return await repo.All<Order>()
              .Where(x => x.Id == Guid.Parse(orderId))
              .Select(o => new OrderViewModel
              {
                  Id = o.Id,
                  OrderNumber = o.Number,
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
                  DriverName = String.Join(" ", o.Driver.User.FirstName, o.Driver.User.LastName),                  
              })
              .FirstOrDefaultAsync();
        }

        public async Task<List<OrderViewModel>> GetOrdersDetails(string orderId)
        {                   
            var order = await repo.All<OrderData>()
                .Where(x => x.OrderId == Guid.Parse(orderId))
                .Include(x => x.Order)
                .ThenInclude(r => r.Address)
                .Include(u => u.User)
                .Select(o => new OrderViewModel
                {
                    Id = o.OrderId,
                    OrderNumber = o.Order.Number,
                    Town = o.Order.Address.Town,
                    Aria = o.Order.Address.Area,
                    Street = o.Order.Address.Street,
                    Number = o.Order.Address.Number,
                    UserName = o.User.Email,
                    PhoneNumner = o.Order.PhoneNumner,
                    PaymentType = o.Order.PaymentType,
                    Price = o.Order.Price,
                    DeliveryPrice = o.Order.DeliveryPrice,
                    RestaurantId = o.Order.RestaurantId,
                    RestaurantName = o.Order.Restaurant.Name,
                    UserId = o.Order.UserCreated.Id,
                    UserCreatedName = o.Order.UserName,
                    Status = o.Status,
                    DataCreated = o.Order.Create,
                    LastStatusTime = o.LastUpdate,
                    DriverName = String.Join(" ", o.Driver.User.FirstName, o.Driver.User.FirstName),
                    TimeForDelivery = o.Order.TimeForDelivery
                }).ToListAsync();

            return order;
        }

        public async Task<OrderStatsModel> GetStats(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = await GetAll(userId, startDate, endDate);

            var delyverySells = orders
                .Where(x => x.Status != Status.Отказана && x.Status != Status.ОтказанаШофьор)
                .Select(x => x.DeliveryPrice).Sum();

            var totalSells = orders
                .Where(x => x.Status != Status.Отказана && x.Status != Status.ОтказанаШофьор)
                .Select(x => x.Price).Sum();

            var ordersCount = orders.Count();

            var newOrdersCount = orders.Where(x => x.Status == Status.Нова).Count();
            var newOrdersProogres = (orders.Where(x => x.Status == Status.Нова).Count() / (double)orders.Count()) * 100;

            var endOrdersCount = orders.Where(x => x.Status == Status.Доставена).Count();
            var endOrdersProogres = (orders
                .Where(x => x.Status == Status.Доставена)
                .Count()
                / (double)orders.Count()) * 100;

            var canceledOrdersCount = orders
                .Where(x => x.Status == Status.Отказана || x.Status == Status.ОтказанаШофьор)
                .Count();
            var canceledOrdersProogres = (orders
                .Where(x => x.Status == Status.Отказана || x.Status == Status.ОтказанаШофьор)
                .Count()
                / (double)orders.Count())
                * 100;

            var acceptedOrdersCount = orders
                .Where(x => x.Status == Status.Приета)
                .Count();
            var acceptedOrdersProogres = (orders
                .Where(x => x.Status == Status.Приета)
                .Count()
                / (double)orders.Count())
                * 100;

            var inProgresOrdersCount = orders
                .Where(x => x.Status == Status.Насочена || x.Status == Status.Изпратена)
                .Count();
            var inProgresOrdersProogres = (orders
                .Where(x => x.Status == Status.Насочена || x.Status == Status.Изпратена)
                .Count()
                / (double)orders.Count())
                * 100;

            var ordersStats = new OrderStatsModel
            {
                OrdersCount = ordersCount,
                NewOrdersCount = newOrdersCount,
                NewOrdersProogres = newOrdersProogres,
                EndOrdersCount = endOrdersCount,
                EndOrdersProogres = endOrdersProogres,
                TotalSells = totalSells,
                DeliverySells = delyverySells,
                CancelledOrdersCount = canceledOrdersCount,
                CancelledOrdersProogres = canceledOrdersProogres,
                AcceptedOrdersCount = acceptedOrdersCount,
                AcceptedOrdersProogres = acceptedOrdersProogres,
                InProgresOrdersCount = inProgresOrdersCount,
                InProgresOrdersProogres = inProgresOrdersProogres
            };

            ordersStats.ChartData.Add("Нова", newOrdersCount);
            ordersStats.ChartData.Add("Приключена", endOrdersCount);
            ordersStats.ChartData.Add("Отказана", canceledOrdersCount);
            ordersStats.ChartData.Add("Приета", acceptedOrdersCount);
            ordersStats.ChartData.Add("В изпълнение", inProgresOrdersCount);

            return ordersStats;
        }

        public async Task<(bool created, string error)> DeliveryOrder(Guid orderId, string userId)
        {
            var error = "";
            var actionResult = false;

            var order = await repo.All<Order>()
                .Where(x => x.Id == orderId)
                .FirstOrDefaultAsync();

            order.Status = Status.Доставена;

            var user = await repo.All<ApplicationUser>()
                  .Include(u => u.Driver)
                  .Where(x => x.Id == userId)
                  .FirstOrDefaultAsync();

            user.Driver.Status = Status.Свободен;

            var orderData = new OrderData()
            {
                OrderId = orderId,
                Order = order,
                LastUpdate = DateTime.Now,
                Status = order.Status,
                UserId = userId,
                User = user
            };

            try
            {
                await repo.AddAsync(orderData);

                repo.SaveChanges();
                actionResult = true;
            }

            catch (Exception)
            {
                error = "Could not save Order";
            }

            return (actionResult, error);
        }

        private (DateTime? startDate, DateTime? endDate) CheckDate(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue) startDate = DateTime.Today;
            if (!endDate.HasValue) endDate = DateTime.Now;

            return (startDate, endDate);
        }
    }
}

