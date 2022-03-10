using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IDriverServices
    {
        public Task<IEnumerable<DriverViewModel>> GetAll();

        public Task<(bool created, string error)> Create(DriverViewModel model);

    }
}
