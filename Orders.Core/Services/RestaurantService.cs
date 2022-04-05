using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;
using Orders.Infrastructure.Data.Repositories;

namespace Orders.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IApplicatioDbRepository repo;
        private readonly UserManager<ApplicationUser> userManager;


        public RestaurantService(
            IApplicatioDbRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<(bool created, string error)> Create(RestaurantViewModel model)
        {
            bool created = false;
            string error = "";

            var address = new Address()
            {
                Town = "София",
                Area = model.Area,
                Street = model.Street,
                Number = model.Number,
                Other = model.AddressOther
            };

            var restaurant = new Restaurant()
            {
                AddressId = address.Id,
                Address = address,
                Name = model.Name,
                Category = model.Category,
                Description = model.Description,
                PhoneNumner = model.PhoneNumner,
                Url = model.Url,
                DataCreated = DateTime.Now
            };

            var user = new ApplicationUser
            {
                Email = model.UserEmail,
                NormalizedEmail = model.UserEmail.ToUpper(),
                UserName = model.UserEmail,
                NormalizedUserName = model.UserEmail.ToUpper(),                
                FirstName = model.FirsName,
                LastName = model.FirsName,
                EmailConfirmed = true,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id
            };

            var test = await userManager.CreateAsync(user, model.UserPassword);

            if (!test.Succeeded)
            {
                error = String.Join(", ", test.Errors.Select(x => x.Description));
                return (created, error);
            }

            test = await userManager.AddToRoleAsync(user, "Restaurant");
            if (!test.Succeeded)
            {
                error = "Could not Create \"Restaurant\" role for User";
                return (created, error);
            }

            return (true, error);
        }

        public async Task<(bool created, string error)> Edit(EditRestaurantViewModel model)
        {
            bool created = false;
            string error = "";
            var tet = model.Id;

            var restaurant = await repo.All<Restaurant>()
                .Include(x => x.Address)
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            restaurant.Address.Area = model.Area;
            restaurant.Address.Street = model.Street;
            restaurant.Address.Number = model.Number;
            restaurant.Name = model.Name;
            restaurant.PhoneNumner = model.PhoneNumner;
            restaurant.Description = model.Description;
            restaurant.Category = model.Category;

            try
            {
                repo.SaveChanges();
                created = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw;
            }

            return (created, error);
        }

        public async Task<IEnumerable<RestaurantViewModel>> GetAll()
        {
            var restaurants = await repo.All<ApplicationUser>()
                .Include(r => r.Restaurant)
                .ThenInclude(a => a.Address)
                .Where(x => x.RestaurantId != null)
                .Select(r => new RestaurantViewModel()
                {
                    Id = (Guid)r.RestaurantId,
                    Name = r.Restaurant.Name,
                    UserEmail = r.Email,
                    Town = r.Restaurant.Address.Town,
                    Area = r.Restaurant.Address.Area,
                    Street = r.Restaurant.Address.Street,
                    Number = r.Restaurant.Address.Number,
                    PhoneNumner = r.Restaurant.PhoneNumner,
                    Category = r.Restaurant.Category,
                    Description = r.Restaurant.Description,
                    Url = r.Restaurant.Url,
                    Created = r.Restaurant.DataCreated
                })
                .ToListAsync();
            return restaurants;
        }

        public async Task<RestaurantViewModel> GetRestaurantById(string restaurantId)
        {
            var restaurant = await repo.All<ApplicationUser>()
                .Include(r => r.Restaurant)
                .ThenInclude(a => a.Address)
                .Where(x => x.RestaurantId.ToString() == restaurantId)
                .Select(r => new RestaurantViewModel()
                {
                    Id = (Guid)r.RestaurantId,
                    Name = r.Restaurant.Name,
                    UserEmail = r.Email,
                    Town = r.Restaurant.Address.Town,
                    Area = r.Restaurant.Address.Area,
                    Street = r.Restaurant.Address.Street,
                    Number = r.Restaurant.Address.Number,
                    PhoneNumner = r.Restaurant.PhoneNumner,
                    Category = r.Restaurant.Category,
                    Description = r.Restaurant.Description,
                    Url = r.Restaurant.Url,
                    Created = r.Restaurant.DataCreated
                })
                .FirstOrDefaultAsync();

            if (restaurant != null)
            {
                throw new ArgumentException("Unknown Restaurant");
            }

            return restaurant;
        }
    }
}



