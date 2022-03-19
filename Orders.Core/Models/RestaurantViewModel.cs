using System.ComponentModel.DataAnnotations;

namespace Orders.Core.Models
{
    public class RestaurantViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public string? Url { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime Created { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumner { get; set; }

        [Required]
        [MaxLength(20)]
        public string Town { get; set; }

        [MaxLength(20)]
        public string? Area { get; set; }

        [Required]
        [MaxLength(20)]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [MaxLength(20)]
        public string? AddressOther { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserEmail { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirsName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserPassword { get; set; }

        [Required]
        [Compare(nameof(UserPassword))]
        public string ConfirmPassword { get; set; }
    }
}
