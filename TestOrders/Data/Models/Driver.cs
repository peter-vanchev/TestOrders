using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class Driver
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Status Status { get; set; }

        public string OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
