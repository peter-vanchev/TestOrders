using System.ComponentModel.DataAnnotations;

namespace TestOrders.Data.Models
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(10)]
        public string Number { get; set; }


        [Required]
        [StringLength(20)]
        public string Brand { get; set; }


        [Required]
        [StringLength(20)]
        public string Model { get; set; }
    }
}
