using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class OrderData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Status Status { get; set; }

        public DateTime Create { get; set; }

        public DateTime LastUpdate { get; set; }

        public string OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public string DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }

        public string RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }
    }
}
