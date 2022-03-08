using System.Linq;

namespace TestOrders.Data.Common
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;


        IQueryable<T> All<T>() where T : class;

        int SaveChanges();

        void Update<T>(T entity) where T : class;
    }
}
