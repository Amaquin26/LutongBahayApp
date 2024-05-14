using LutongBahayApp.Models;

namespace LutongBahayApp.ViewModels
{
    public class BasketItemViewModel
    {
        public MarketInfoViewModel MarketInfo { get; set; }
        public List<GroupedFoodItemViewModel> BasketItems { get; set; }
    }
}
