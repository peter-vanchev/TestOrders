using Orders.Core.Models;

namespace Orders.Core.Contracts
{
    public interface IDriverServices
    {
        public Task<IEnumerable<DriverViewModel>> GetAll();

        public Task<(bool created, string error)> Create(DriverViewModel model);

        Task<(bool created, string error)> AsignDriver(OrderViewModel model);

        Task<IEnumerable<DriverViewModel>> GetFreeDrivers();
    }
}
