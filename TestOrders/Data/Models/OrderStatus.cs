using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class OrderStatus
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Status Status { get; set; }

        public DateTime Time { get; set; }

        public string OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public string DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }
    }
}
