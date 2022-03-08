using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IRestaurantService
    {
        IEnumerable<ObjectViewModel> GetAll();

        public (bool created, string error) Create(RestaurantViewModel model);

        public (bool created, string error) Delete(string restaurantId);

    }
}
