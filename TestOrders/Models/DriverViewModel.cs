using TestOrders.Data.Models;

namespace TestOrders.Models
{
    public class DriverViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string OrderId { get; set; }

        public Status Status { get; set; }

    }
}
