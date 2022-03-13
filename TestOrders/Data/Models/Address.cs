using System.ComponentModel.DataAnnotations;

namespace TestOrders.Data.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(20)]
        public string Town { get; set; } = "София";

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }
    }
}
