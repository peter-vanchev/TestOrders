using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Orders = new HashSet<Order>();
        }

        public Guid? RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public Guid? DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
