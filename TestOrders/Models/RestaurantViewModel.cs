namespace TestOrders.Models
{
    public class RestaurantViewModel
    {
<<<<<<< HEAD
        public string Id { get; set; }
=======
        public string Id { get; set; } = Guid.NewGuid().ToString();
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Category { get; set; }

        public string PhoneNumner { get; set; }

        public string Town { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }
<<<<<<< HEAD

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }
=======
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
    }
}
