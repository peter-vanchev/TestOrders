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

        [Required]
        public DateTime LastUpdate { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public Guid? DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver? Driver { get; set; }

        [MaxLength(100)]
        public string Logs { get; set; }
    }
}
