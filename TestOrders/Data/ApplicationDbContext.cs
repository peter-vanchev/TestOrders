using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestOrders.Data.Models;
using TestOrders.Models;

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
            base.OnModelCreating(builder);

            builder.Entity<ProductOrder>()
                .HasKey(t => new { t.RestaurantId, t.ProductId });
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public Driver Drivers { get; set; }

        public DbSet<TestOrders.Models.DriverViewModel> DriverViewModel { get; set; }
    }
}