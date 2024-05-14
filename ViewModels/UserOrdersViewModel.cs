using LutongBahayApp.Data.Enum;

namespace LutongBahayApp.ViewModels
{
    public class UserOrdersViewModel
    {
        public int Id { get; set; }
        public string OrderAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
