using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestOrders.Data.Models;

namespace TestOrders.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            builder.Entity<ProductOrder>()
                .HasKey(t => new { t.OrderId, t.ProductId });
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public DbSet<OrderData> OrderDatas { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Car> Cars { get; set; }
    }
}