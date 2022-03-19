using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Infrastructure.Data.Models
{
    public class Driver
    {
        public Driver()
        {
            this.Orders = new HashSet<Order>();
        }

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

        public ICollection<Order> Orders { get; set; }

        public ApplicationUser User { get; set; }
    }
}
