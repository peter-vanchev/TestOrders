using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class Driver
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }

        [Required]
        public Status Status { get; set; }

        public DateTime DataCreated { get; set; }

        public Guid? CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; }

        public Guid? OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
