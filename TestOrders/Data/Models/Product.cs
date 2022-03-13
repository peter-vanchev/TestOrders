using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.ProductOrder = new HashSet<ProductOrder>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Url { get; set; }

        public DateTime DataCreated { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public Guid RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}