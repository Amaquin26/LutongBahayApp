using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;

namespace LutongBahayApp.Interfaces
{
    public interface IMarketRepository
    {
        public Task<List<MarketDashboardViewModel>> GetUserMarket();
        public Task<int> AddMarket(string name, string address, string landmark);
        public Task<(bool, MarketPageInfoViewModel)> GetMarketPageInfo(int id);
        public Task<(bool, int)> EditMarket(int id, string name, string address, string landmark);
        public Task<(bool, int)> AddFoodToMarket(int id, string name, decimal price, string description, IFormFile filepath);
        public Task<int> VerifyFoodOwnerShip(int marketId, int foodId);
        public Task<ManageFoodViewModel> GetFoodManageInfo(int foodId);
        public Task<(bool, int)> EditFoodInMarket(int marketId, int foodId, string name, decimal price, string description, IFormFile filepath);
        public Task<List<OrderFoodItemsViewModel>> GetMarketFoodOrders();
        public Task<int> ChangeStatus(int foodId, int orderId, int flag);
    }
}
