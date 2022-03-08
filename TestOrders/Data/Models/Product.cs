using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.ProductOrder = new HashSet<ProductOrder>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public string RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}