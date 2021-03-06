using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Data.Models;

namespace Orders.Infrastructure.Data
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

            builder.Entity<ProductOrder>()
                .HasKey(t => new { t.OrderId, t.ProductId });  
                       
            base.OnModelCreating(builder);
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