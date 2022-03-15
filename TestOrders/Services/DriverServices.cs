using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestOrders.Contracts;
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Services
{
    public class DriverServices : IDriverServices
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public DriverServices(IRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }


        public async Task<IEnumerable<DriverViewModel>> GetAll()
        {
            var drivers = await repo.All<ApplicationUser>()
                .Include(x => x.Driver)
                .ThenInclude(x => x.Car)
                .Where(x => x.Driver != null)
                 .Select(x => new DriverViewModel()
                 { 
                     Name = x.Driver.Name,
                     PhoneNumber = x.PhoneNumber,
                     CarModel = x.Driver.Car.Model,
                     CarNumber = x.Driver.Car.Number,
                     CarType = x.Driver.Car.Type, 
                     CarUrl = x.Driver.Car.Url,
                     Status = x.Driver.Status                        
                 }).ToListAsync();

            return drivers;
        }          

        public async Task<(bool created, string error)> Create(DriverViewModel model)
        {
            bool created = true;
            string error = null;

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
                Name = model.Name,
                DataCreated = DateTime.Now,
                Status = Status.Свободен,
                Car = car,
                CarId = car.Id                
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
            };

            try
            {
                var test = await userManager.CreateAsync(user, model.Password);
                created = true;
            }
            catch (Exception)
            {
                created = false; 
                error = "Could not Create Driver";
            }

            try
            {
                await userManager.AddToRoleAsync(user, "Driver");
            }
            catch (Exception)
            {
                created = false;
                error = "Could not Create \"Driver\" role for User";
            }

            return (created, error);
        }
    }
}
