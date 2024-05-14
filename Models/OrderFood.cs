using LutongBahayApp.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class OrderFood
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set;}
        public Food Food { get; set;}
        public Order Order { get; set; }
        public int Quantity { get; set; }
        public FoodOrderStatus Status { get; set; } = FoodOrderStatus.Pending;
        public decimal Price { get; set; }
    }
}
