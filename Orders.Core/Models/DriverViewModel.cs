using Orders.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Orders.Core.Models
{
    public class DriverViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string DriverUrl { get; set; }

        public string? CarModel { get; set; }

        public string CarNumber { get; set; }

        public string CarType { get; set; }

        public string CarUrl { get; set; }

        public Guid OrderId { get; set; }

        public Status Status { get; set; }
    }
}
