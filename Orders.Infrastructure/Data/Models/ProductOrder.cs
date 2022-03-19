using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Infrastructure.Data.Models
{
    public class ProductOrder
    {
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

    }
}
