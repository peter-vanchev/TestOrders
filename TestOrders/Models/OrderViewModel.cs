using System;
using TestOrders.Data.Models;

namespace TestOrders.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; } 

        public string Town { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Email { get; set; }

        public string PhoneNumner { get; set; }

        public string PaymentType { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        public string RestaurantId { get; set; }

        public string RestaurantName { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public Status Status { get; set; }

        public DateTime LastStatusTime { get; set; }

        public string DriverId { get; set; }

        public Driver Driver { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
