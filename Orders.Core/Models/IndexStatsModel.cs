namespace Orders.Core.Models
{
    public class IndexStatsModel
    {
        public IndexStatsModel()
        {
            ChartData = new Dictionary<string, int>();
        }

        public decimal TotalSells { get; set; }

        public decimal DeliverySells { get; set; }

        public int OrdersCount { get; set; }

        public int NewOrdersCount { get; set; }

        public double NewOrdersProogres { get; set; }

        public int AcceptedOrdersCount { get; set; }

        public double AcceptedOrdersProogres { get; set; }

        public int InProgresOrdersCount { get; set; }

        public double InProgresOrdersProogres { get; set; }

        public int EndOrdersCount { get; set; }

        public double EndOrdersProogres { get; set; }

        public int CancelledOrdersCount { get; set; }

        public double CancelledOrdersProogres { get; set; }

        public Dictionary<string, int> ChartData { get; set; } 
    }
}
