using System.ComponentModel.DataAnnotations.Schema;

namespace TestOrders.Data.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            this.Products = new HashSet<Product>();
            this.Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Category { get; set; }

        public string PhoneNumner { get; set; }

        public string AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
