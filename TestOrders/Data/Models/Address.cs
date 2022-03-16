using System.ComponentModel.DataAnnotations;

namespace TestOrders.Data.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Town { get; set; } = "София";

        [MaxLength(50)]
        public string Area { get; set; }

        [Required]
        [MaxLength(20)]
        public string StreetNumber { get; set; }

        [MaxLength(20)]
        public string Other { get; set; }
    }
}
