using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class OrderData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Status Status { get; set; }

        public DateTime Create { get; set; }

        public DateTime LastUpdate { get; set; }

        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public Guid? DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }

        public Guid? RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }
    }
}
