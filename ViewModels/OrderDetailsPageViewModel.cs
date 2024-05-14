using LutongBahayApp.Data.Enum;

namespace LutongBahayApp.ViewModels
{
    public class OrderDetailsPageViewModel
    {
        public List<OrderDetailsViewModel> Foods { get; set; }
        public string OrderAddress { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
