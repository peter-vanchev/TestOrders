using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IRestaurantService
    {
        public Task<IEnumerable<ObjectViewModel>> GetAll();

        public Task<(bool created, string error)> Create(RestaurantViewModel model);

        public (bool created, string error) Delete(string restaurantId);
    }
}
