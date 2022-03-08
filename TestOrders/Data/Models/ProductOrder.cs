using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class ProductOrder
    {
        public string RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public string ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
