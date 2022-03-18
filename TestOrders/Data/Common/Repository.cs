using Microsoft.EntityFrameworkCore;

namespace TestOrders.Data.Common
{
    public class Repository : IRepository
    {
        private readonly DbContext dbContext;

        public Repository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public void Add<T>(T entity) where T : class
        {
            DbSet<T>().Add(entity);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>().AsQueryable();
        }

        public async Task<IQueryable<T>> AllAsync<T>() where T : class
        {
            var set = await DbSet<T>().ToListAsync();
            return set.AsQueryable();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            DbSet<T>().Update(entity);
        }

        private DbSet<T> DbSet<T>() where T : class 
        { 
            return dbContext.Set<T>(); 
        }
    }
}
