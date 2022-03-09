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
            var drivers = await repo.All<Driver>()
                .Include(x => x.User)
                .Select(p => new DriverViewModel()
                {
                    Name = p.User.UserName,
                    Email = p.User.Email,
                    OrderId = p.OrderId,
                    Status = p.Status
                })
                .ToListAsync();

            return drivers;
        }

        public async Task<(bool created, string error)> Create(DriverViewModel model)
        {
            bool created = true;
            string error = null;

            var user = new ApplicationUser
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                EmailConfirmed = true
            };

            var driver = new Driver()
            {
                User = user,
                UserId = user.Id,
                Status = Status.Свободен                
            };

            //user.Driver = driver;
            //user.DriverId = driver.Id;

            try
            {
                await userManager.CreateAsync(user, model.Password);
                repo.Add(driver);
                repo.SaveChanges();
                await userManager.AddToRoleAsync(user, "Driver");
                created = true;
            }
            catch (Exception)
            {
                error = "Could not Create Driver";
            }

            return (created, error);
        }
    }
}
