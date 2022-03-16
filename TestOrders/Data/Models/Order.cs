using System.ComponentModel.DataAnnotations;
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

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumner { get; set; }

        [Required]
        [MaxLength(10)]
        public string PaymentType { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        public decimal DeliveryPrice { get; set; }

        [Required]
        [Range(0, 1000)]
        public int TimeForDelivery { get; set; }

        [Required]
        public Status Status { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public DateTime DataCreated { get; set; }

        public Guid RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public Guid AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser UserCreated { get; set; }

        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
