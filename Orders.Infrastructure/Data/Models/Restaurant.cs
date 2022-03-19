using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Infrastructure.Data.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            this.Products = new HashSet<Product>();
            this.Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Url { get; set; }

        [Required]
        [MaxLength(20)]
        public string Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumner { get; set; }

        public DateTime DataCreated { get; set; }

        public Guid AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
