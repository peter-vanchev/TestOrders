using Orders.Core.Models;

namespace Orders.Core.Contracts
{
    public interface IDriverServices
    {
        Task<IEnumerable<DriverViewModel>> GetAll();

        Task<(bool created, string error)> Create(DriverViewModel model);

        Task<(bool created, string error)> AsignDriver(Guid driverId, Guid orderId, string userId);

        Task<DriverViewModel> GetDriver(string Id);

        Task<IEnumerable<DriverViewModel>> GetFreeDrivers();
    }
}
