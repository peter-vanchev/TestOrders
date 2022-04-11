using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
using Orders.Infrastructure.Data.Repositories;

namespace Orders.Core.Services
{
    public class DriverServices : IDriverServices
    {
        private readonly IApplicatioDbRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public DriverServices(IApplicatioDbRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<(bool created, string error)> AsignDriver(Guid driverId, Guid orderId, string userId)
        {
            bool created = true;
            string error = "";

            var driver = await repo.All<Driver>()
                .Where(x => x.Id == driverId)
                .FirstOrDefaultAsync();

            Order? order = await repo.All<Order>()
                .Where(x => x.Id == orderId)
                .FirstOrDefaultAsync();

            var user = await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var orderStatus = new OrderData
            {
                DriverId = driverId,
                Driver = driver,
                Order = order,
                OrderId = orderId,
                Status = Status.Насочена,
                LastUpdate = DateTime.Now,
                UserId = userId,
                User = user
            };

            order.Status = Status.Насочена;
            driver.Status = Status.Зает;
            order.Driver = driver;
            order.DriverId = driver.Id;

            try
            {
                await repo.AddAsync(orderStatus);

                repo.SaveChanges();
                created = true;
            }

            catch (Exception)
            {
                error = "Could not save Order";
            }

            return (created, error);
        }

        public async Task<(bool created, string error)> CreateAsync(DriverViewModel model)
        {
            bool created = true;
            var error = new List<string>();

            var car = new Car()
            {
                Brand = model.CarModel,
                Model = model.CarModel,
                Number = model.CarNumber,
                Type = model.CarType,
                Url = model.CarUrl
            };

            var driver = new Driver()
            {
                DataCreated = DateTime.Now,
                Status = Status.Свободен,
                Url = model.DriverUrl,
                Car = car,
                CarId = car.Id,
            };

            var user = new ApplicationUser
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                EmailConfirmed = true,
                Driver = driver,
                DriverId = driver.Id,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var addUser = await userManager.CreateAsync(user, model.Password);

            if (!addUser.Succeeded)
            {
                var result = addUser.Errors.ToList();
                foreach (var err in result)
                {
                    error.Add(err.Description);
                }

                created = false;
                return (created, String.Join(", ", error));
            }

            var addRole = await userManager.AddToRoleAsync(user, "Driver");
            if (!addRole.Succeeded)
            {
                var result = addUser.Errors.ToList();

                foreach (var err in result)
                {
                    error.Add(err.Description);
                }

                created = false;
                return (created, String.Join(", ", error));
            }

            return (created, "");
        }

        public async Task<(bool edited, string error)> EditAsync(EditDriverViewModel model)
        {
            bool edited = false;
            string error = "";

            var driver = await repo.All<ApplicationUser>()
               .Include(x => x.Driver)
               .ThenInclude(car => car.Car)
               .Where(x => x.DriverId == model.Id)
               .FirstOrDefaultAsync();

            driver.FirstName = model.FirstName;
            driver.LastName = model.LastName;
            driver.PhoneNumber = model.PhoneNumber;
            driver.Driver.Car.Type = model.CarType;
            driver.Driver.Car.Model = model.CarModel;
            driver.Driver.Car.Number = model.CarNumber;

            try
            {
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

        public async Task<IEnumerable<DriverViewModel>> GetAllAsyncl()
        {
            var drivers = await repo.All<ApplicationUser>()
                .Include(x => x.Driver)
                .ThenInclude(x => x.Car)
                .Where(x => x.Driver != null)
                 .Select(x => new DriverViewModel()
                 { 
                     Id = x.Driver.Id,
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     PhoneNumber = x.PhoneNumber,
                     Email = x.Email,
                     DriverUrl = x.Driver.Url,
                     CarModel = x.Driver.Car.Model,
                     CarNumber = x.Driver.Car.Number,
                     CarType = x.Driver.Car.Type, 
                     CarUrl = x.Driver.Car.Url,
                     Status = x.Driver.Status                        
                 }).ToListAsync();

            return drivers;
        }

        public async Task<DriverViewModel> GetDriverByIdAsync(Guid driverId)
        {
            var drivers = await repo.All<ApplicationUser>()
                .Include(x => x.Driver)
                .ThenInclude(x => x.Car)
                .Where(x => x.DriverId == driverId)
                 .Select(x => new DriverViewModel()
                 {
                     Id = x.Driver.Id,
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     PhoneNumber = x.PhoneNumber,
                     Email = x.Email,
                     DriverUrl = x.Driver.Url,
                     CarModel = x.Driver.Car.Model,
                     CarNumber = x.Driver.Car.Number,
                     CarType = x.Driver.Car.Type,
                     CarUrl = x.Driver.Car.Url,
                     Status = x.Driver.Status
                 }).FirstOrDefaultAsync();

            return drivers;
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
                    Status = x.Driver.Status,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                })
                .ToListAsync();

            return drivers;
        }

    }
}
