using Orders.Core.Models;

namespace Orders.Core.Contracts
{
    public interface IProductService
    {
        IEnumerable<ProductViewModel> GetAll();

        IEnumerable<ProductViewModel> GetAll(string restaurantId);

        Task<IEnumerable<ProductViewModel>> GetAllAsync(string restaurantId);

        Task<(bool created, string? error)> Create(ProductViewModel model);

    }
}
