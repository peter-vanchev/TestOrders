using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Core.Models
{
    public class OrderStatsModel
    {
        public decimal TotalSells { get; set; }

        public int OrdersCount { get; set; }

        public int NewOrdersCount { get; set; }

        public double NewOrdersProogres { get; set; }

        public int EndOrdersCount { get; set; }

        public double EndOrdersProogres { get; set; }

        public int CancelledOrdersCount { get; set; }

        public double CancelledOrdersProogres { get; set; }

    }
}
