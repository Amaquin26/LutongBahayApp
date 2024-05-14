using LutongBahayApp.ViewModels;
using LutongBahayApp.ViewModels.Post;

namespace LutongBahayApp.Interfaces
{
    public interface IShopRepository
    {
        public Task<List<FoodListItemViewModel>> GetFoodsForShop();
        public Task<FoodItemViewModel> GetIndividualFood(int id);
        public Task<MarketPageInfoViewModel> GetMarketPageInfo(int id, string foodName = null, int foodId = 0);
        public Task PostReview(PostReviewViewModel model);
    }
}
