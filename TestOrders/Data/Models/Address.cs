namespace TestOrders.Data.Models
{
    public class Address
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Town { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }
    }
}
