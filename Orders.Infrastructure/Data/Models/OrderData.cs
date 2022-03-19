using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Infrastructure.Data.Models
{
    public class OrderData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Status Status { get; set; }

        public DateTime LastUpdate { get; set; }

        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public Guid? DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver? Driver { get; set; }
    }
}
