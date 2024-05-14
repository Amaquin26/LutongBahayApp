using LutongBahayApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public string OrderAdress { get; set; }
        public decimal OrderAmountDue { get; set; }
        [ForeignKey("AspNetUsers")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<OrderFood> OrderFood { get; set; } 
    }
}
