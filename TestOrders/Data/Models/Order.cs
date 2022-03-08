using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.ProductOrder = new HashSet<ProductOrder>();
            this.Status = Status.Нова;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Email { get; set; }

        public string PhoneNumner { get; set; }

        public string PaymentType { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        public decimal DeliveryPrice { get; set; }

        public Status Status { get; set; }

        public string RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public string AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
