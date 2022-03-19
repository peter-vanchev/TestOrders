using System.ComponentModel.DataAnnotations;

namespace Orders.Infrastructure.Data.Models
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
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [MaxLength(20)]
        public string Other { get; set; }
    }
}
