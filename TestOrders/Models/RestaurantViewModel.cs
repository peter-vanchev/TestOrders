namespace TestOrders.Models
{
    public class RestaurantViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Category { get; set; }

        public string PhoneNumner { get; set; }

        public string Town { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }
    }
}
