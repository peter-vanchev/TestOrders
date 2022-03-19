using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Infrastructure.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.ProductOrder = new HashSet<ProductOrder>();
            this.OrderData = new HashSet<OrderData>();
            this.Status = Status.Нова;
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Create { get; set; }

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

        [Required]
        public Guid AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }

        [Required]
        public Guid RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public Guid? DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver? Driver { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser UserCreated { get; set; }

        public ICollection<ProductOrder> ProductOrder { get; set; }

        public ICollection<OrderData> OrderData { get; set; }
    }
}
