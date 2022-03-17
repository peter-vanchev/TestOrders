using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> GetAll();

        Task<(bool created, string error)> Create(OrderViewModel model, string userId);

        Task<OrderViewModel> GetOrderById(string orderId);

        Task<(bool created, string error)> AsignDriver(OrderViewModel model);

        Task<IEnumerable<DriverViewModel>> GetFreeDrivers();
    }
}
