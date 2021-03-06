using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;

namespace Orders.Core.Contracts
{
    public interface IOrderService
    {
        Task<(bool, string)> AcceptOrder(string userId, string orderId, bool action);

        Task<(bool created, string error)> CreateAsync(OrderViewModel model, string userId);

        Task<(bool edited, string error)> EditAsync(OrderViewModel model, string userId);

        Task<IEnumerable<OrderViewModel>> GetAll(DateTime? startDate = null, DateTime? endDate = null);

        Task<IEnumerable<OrderViewModel>> GetAll(string userId, DateTime? startDate = null, DateTime? endDate = null);

        Task<OrderViewModel> GetOrderById(string orderId);

        Task<List<OrderViewModel>> GetOrdersDetails(string orderId);

        Task<IndexStatsModel> GetStats(string userId, DateTime? startDate = null, DateTime? endDate = null);

        Task<(bool created, string error)> DeliveryOrder(Guid orderId, string userId);
    }
}
