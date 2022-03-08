using TestOrders.Contracts;
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository repo;

        public RestaurantService(IRepository _repo)
        {
            repo = _repo;
        }

        public (bool created, string error) Create(RestaurantViewModel model)
        {
            bool created = true;
            string error = null;

            //var (isValid, validationError) = validationService.ValidateModel(model);
            var address = new Address()
            {
                Town = model.Town,
                Number = model.Number,
                Street = model.Street
            };

            var restaurant = new Restaurant()
            {
                AddressId = address.Id,
                Address = address,
                Name = model.Name,
                Category = model.Category,
                Description = model.Description,
                PhoneNumner = model.PhoneNumner,
                Url = model.Url                
            };

            try
            {
                repo.Add(restaurant);
                repo.SaveChanges();
                created = true;
            }
            catch (Exception)
            {
                error = "Could not save product";
            }

            return (created, error);
        }

        public (bool created, string error) Delete(string restaurantId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ObjectViewModel> GetAll()
        {
            var restaurant = repo.All<Restaurant>()
                .Select(p => new ObjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    Url = p.Url                    
                })
                .ToList();

            return restaurant;
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


    }
}


