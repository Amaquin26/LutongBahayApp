using LutongBahayApp.ViewModels;

namespace LutongBahayApp.Interfaces
{
    public interface IShopRepository
    {
        public Task<List<FoodListItemViewModel>> GetFoodsForShop();
        public Task<FoodItemViewModel> GetIndividualFood(int id);
        public Task<MarketPageInfoViewModel> GetMarketPageInfo(int id, string foodName = null, int foodId = 0);
    }
}
