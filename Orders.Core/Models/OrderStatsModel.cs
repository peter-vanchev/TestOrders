namespace Orders.Core.Models
{
    public class OrderStatsModel
    {
        public OrderStatsModel()
        {
            MyProperty = new Dictionary<string, int>();
            MyProperty.Add("edno", 1);
            MyProperty.Add("dve", 2);
        }
        public decimal TotalSells { get; set; }

        public decimal DeliverySells { get; set; }

        public int OrdersCount { get; set; }

        public int NewOrdersCount { get; set; }

        public double NewOrdersProogres { get; set; }

        public int EndOrdersCount { get; set; }

        public double EndOrdersProogres { get; set; }

        public int CancelledOrdersCount { get; set; }

        public double CancelledOrdersProogres { get; set; }

        public Dictionary<string, int> MyProperty { get; set; } 
    }
}
