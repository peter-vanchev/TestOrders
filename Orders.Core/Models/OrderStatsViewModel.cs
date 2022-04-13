using Orders.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Core.Models
{
    public class OrderStatsViewModel
    {
        public Guid Id { get; set; }

        public int OrderNumber { get; set; }

        public string UserName { get; set; }

        public string PaymentType { get; set; }

        public int TimeForDelivery { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public Status Status { get; set; }


        public DateTime DataCreated { get; set; }

        public DateTime LastStatusTime { get; set; }

        public Guid? RestaurantId { get; set; }

        public string RestaurantName { get; set; }

        public string UserId { get; set; }

        public string UserCreatedName { get; set; }

        public Guid? DriverId { get; set; }

        public string DriverName { get; set; }
    }
}
