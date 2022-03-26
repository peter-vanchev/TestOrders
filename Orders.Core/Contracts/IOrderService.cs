using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;

namespace Orders.Core.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> GetAll(string userId);

        Task<(bool created, string error)> Create(OrderViewModel model, string userId);

        Task<OrderViewModel> GetOrderById(string orderId);

        Task<(bool, string)> AcceptOrder(string userId, string orderId, bool action);

        Task<(bool created, string error)> DeliveryOrder(Guid orderId, string userId);
    }
}
