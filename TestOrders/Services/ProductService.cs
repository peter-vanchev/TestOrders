using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestOrders.Contracts;
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductService(IRepository _repo,
            UserManager<ApplicationUser> _userManager,
            IRestaurantService _restaurantService)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public IEnumerable<ProductViewModel> GetAll()
        {
            var products = repo.All<Product>()
                .Select(x => new ProductViewModel { 
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category,
                    Description = x.Description,
                    Price = x.Price,
                    Url = x.Url
                }).ToList();

            return products;
        }

        public IEnumerable<ProductViewModel> GetAll(string restourantId)
        {
            if (restourantId == null)
            {
                return GetAll();
            }

            var products = repo.All<Product>()
                .Include(x => x.Restaurant)
                .Where(x => x.RestaurantId == restourantId)
                .ToList();

            var menu = new List <ProductViewModel>();

            foreach (var item in products)
            {
                var product = new ProductViewModel() { 
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Category = item.Category,
                    Price = item.Price,
                    Url = item.Url
                };
                menu.Add(product);
            }
            return menu;
        }

        public Task<IEnumerable<ProductViewModel>> GetAllAsync(string restaurantId)
        {
            throw new NotImplementedException();
        }


        public (bool created, string error) Create(ProductViewModel model)
        {
            bool created = true;
            string error = null;

            var product = new Product()
            {
                Name = model.Name,
                Category = model.Category,
                Description= model.Description,
                Url = model.Url,
                Price = model.Price,
            };

            try
            {
                repo.Add(product);
                repo.SaveChanges();
                created = true;
            }
            catch (Exception)
            {
                error = "Could not save product";
            }

            return (created, error);
        }

    }
}
