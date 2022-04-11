using Orders.Core.Models;

namespace Orders.Core.Contracts
{
    public interface IRestaurantService
    {
        public Task<IEnumerable<RestaurantViewModel>> GetAllAsync();

        public Task<(bool created, string error)> Create(RestaurantViewModel model);

        public Task<RestaurantViewModel> GetRestaurantById(string restaurantId);

        public Task<(bool created, string error)> EditAsync(EditRestaurantViewModel model);
    }
}
