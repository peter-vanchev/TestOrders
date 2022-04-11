using Orders.Core.Models;

namespace Orders.Core.Contracts
{
    public interface IDriverServices
    {
        Task<(bool created, string error)> AsignDriver(Guid driverId, Guid orderId, string userId);

        Task<(bool created, string error)> CreateAsync(DriverViewModel model);

        Task<(bool edited, string error)> EditAsync(EditDriverViewModel model);

        Task<IEnumerable<DriverViewModel>> GetAllAsyncl();

        Task<DriverViewModel> GetDriverByIdAsync(Guid driverId);

        Task<IEnumerable<DriverViewModel>> GetFreeDrivers();
    }
}
