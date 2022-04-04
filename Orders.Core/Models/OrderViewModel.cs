using Orders.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Orders.Core.Models
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public int OrderNumber { get; set; }

        [MaxLength(20)]
        public string? Town { get; set; }

        [MaxLength(50)]
        public string? Aria { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [MaxLength(50)]
        public string? AddressOther { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumner { get; set; }

        [Required]
        [MaxLength(10)]
        public string PaymentType { get; set; }

        [Required]
        public int TimeForDelivery { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }
        
        public Status Status { get; set; }

        public string? Description { get; set; }

        public DateTime DataCreated { get; set; }

        public DateTime LastStatusTime { get; set; }

        public Guid? RestaurantId { get; set; }

        public string? RestaurantName { get; set; }

        public string? UserId { get; set; }

        public string? UserCreatedName { get; set; }

        public Guid? DriverId { get; set; }

        public string? DriverName { get; set; }
    }
}
