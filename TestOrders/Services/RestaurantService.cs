using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestOrders.Contracts;
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;


        public RestaurantService(
            IRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<(bool created, string error)> Create(RestaurantViewModel model)
        {
            bool created = false;
            string error = null;

            var address = new Address()
            {
                Town = "София",
                StreetNumber = model.Street,
                Area = model.Area,
                Other = ""
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
                EmailConfirmed = true,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id
            };

            var test = await userManager.CreateAsync(user, model.UserPassword);

            if (!test.Succeeded)
            {
                error = "Could not Create Restaurant";
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

        public (bool created, string error) Delete(string restaurantId)
        {
            throw new NotImplementedException();
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
                    Street = r.Restaurant.Address.StreetNumber,
                    PhoneNumner = r.Restaurant.PhoneNumner,
                    Category = r.Restaurant.Category,
                    Description = r.Restaurant.Description,
                    Url = r.Restaurant.Url,
                    Created = r.Restaurant.DataCreated
                })
                .ToListAsync();
            return restaurants;
        }

        public IEnumerable<ProductViewModel> GetMenu(string restorantId)
        {
            var restaurant = repo.All<Product>()

                .Select(p => new ProductViewModel()
                {
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    Url = p.Url
                })
                .ToList();

            return restaurant;
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
                    Street = r.Restaurant.Address.StreetNumber,
                    PhoneNumner = r.Restaurant.PhoneNumner,
                    Category = r.Restaurant.Category,
                    Description = r.Restaurant.Description,
                    Url = r.Restaurant.Url,
                    Created = r.Restaurant.DataCreated
                })
                .FirstOrDefaultAsync();
            return restaurant;
        }
    }
}



