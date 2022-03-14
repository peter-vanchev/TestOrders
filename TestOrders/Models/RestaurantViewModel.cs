using System.ComponentModel.DataAnnotations;

namespace TestOrders.Models
{
    public class RestaurantViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public string Url { get; set; }

        public string Category { get; set; }

        public string Created { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumner { get; set; }

        [Required]
        [MaxLength(20)]
        public string Town { get; set; }

        [Required]
        [MaxLength(20)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string Number { get; set; }


        [Required]
        [MaxLength(100)]
        public string UserEmail { get; set; }

        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserPassword { get; set; }

        [Required]
        [Compare(nameof(UserPassword))]
        public string ConfirmPassword { get; set; }
    }
}
