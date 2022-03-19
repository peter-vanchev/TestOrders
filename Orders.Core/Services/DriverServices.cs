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


        public async Task<IEnumerable<DriverViewModel>> GetAll()
        {
            var drivers = await repo.All<ApplicationUser>()
                .Include(x => x.Driver)
                .ThenInclude(x => x.Car)
                .Where(x => x.Driver != null)
                 .Select(x => new DriverViewModel()
                 { 
                     Id = x.Driver.Id,
                     Name = x.Driver.Name,
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

        public async Task<(bool created, string error)> Create(DriverViewModel model)
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
                Name = model.Name,
                DataCreated = DateTime.Now,
                Status = Status.Свободен,
                Url = model.DriverUrl,
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

            var addUser = await userManager.CreateAsync(user, model.Password);

            if (!addUser.Succeeded)
            {                   
                var result  = addUser.Errors.ToList();
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

            return (created, null);
        }
    }
}
