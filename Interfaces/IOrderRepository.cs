using LutongBahayApp.ViewModels;
using LutongBahayApp.ViewModels.Post;

namespace LutongBahayApp.Interfaces
{
    public interface IOrderRepository
    {
        public Task<bool> CheckOutFoods(CheckOutViewModel checkout);
        public Task<List<UserOrdersViewModel>> GetUserOrders();
        public Task<OrderDetailsPageViewModel> GetOrderDetails(int id);
        public Task<bool> CancelOrder(int id);
    }
}
