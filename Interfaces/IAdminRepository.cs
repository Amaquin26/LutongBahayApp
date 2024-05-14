using LutongBahayApp.ViewModels;

namespace LutongBahayApp.Interfaces
{
    public interface IAdminRepository
    {
        public Task<List<UserOrdersViewModel>> GetOnDeliveryOrders();
        public Task SetMarketDelivery(int flag, int orderId);
    }
}
