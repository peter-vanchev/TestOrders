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
                .Where(x => x.Driver != null)
                .Select(p => new DriverViewModel()
                {
                    Name = p.UserName,
                    Email = p.Email,
                    OrderId = (Guid)p.Driver.OrderId,
                    Status = p.Driver.Status
                })
                .ToListAsync();

            return drivers;
        }

        public async Task<(bool created, string error)> Create(DriverViewModel model)
        {
            bool created = true;
            string error = null;

            var driver = new Driver()
            {
                Status = Status.Свободен
            };

            var user = new ApplicationUser
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                EmailConfirmed = true,
                Driver = driver,
                DriverId = driver.Id
            };

            try
            {
                await userManager.CreateAsync(user, model.Password);
                created = true;
            }
            catch (Exception)
            {
                error = "Could not Create Driver";
            }

            try
            {
                await userManager.AddToRoleAsync(user, "Driver");
            }
            catch (Exception)
            {
                error = "Could not Create \"Restaurant\" role for User";
            }

            return (created, error);
        }
    }
}
