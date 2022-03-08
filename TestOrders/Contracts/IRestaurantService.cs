using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IRestaurantService
    {
        IEnumerable<ObjectViewModel> GetAll();

<<<<<<< HEAD
        public Task<(bool created, string error)> Create(RestaurantViewModel model);
=======
        public (bool created, string error) Create(RestaurantViewModel model);
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83

        public (bool created, string error) Delete(string restaurantId);

    }
}
