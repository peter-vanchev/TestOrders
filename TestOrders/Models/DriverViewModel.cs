using System.ComponentModel.DataAnnotations;
using TestOrders.Data.Models;

namespace TestOrders.Models
{
    public class DriverViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string CarModel { get; set; }

        public string CarNumber { get; set; }

        public string CarType { get; set; }

        public string CarUrl { get; set; }

        public Guid OrderId { get; set; }

        public Status Status { get; set; }
    }
}
