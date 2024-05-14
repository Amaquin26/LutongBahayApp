using LutongBahayApp.Models;

namespace LutongBahayApp.ViewModels
{
    public class ManageFoodViewModel
    {
        public FoodItemViewModel FoodItemViewModel { get; set; }
        public List<SoldFoodMetric> SoldFoodMetric { get; set; }
        public List<FoodPriceChange> FoodPriceChange { get; set; }
        public double PriceGrowth { get; set; }
        public double SalesGrowth { get; set; }
        public int TotalSold { get; set; }
    }
}
