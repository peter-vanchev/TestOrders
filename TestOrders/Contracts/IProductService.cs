using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IProductService
    {
        IEnumerable<ProductViewModel> GetAll();

        IEnumerable<ProductViewModel> GetAll(string restaurantId);

        Task<IEnumerable<ProductViewModel>> GetAllAsync(string restaurantId);

        public (bool created, string error) Create(ProductViewModel model);

    }
}
