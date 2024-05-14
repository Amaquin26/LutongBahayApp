using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class BasketFood
    {
        [ForeignKey("Basket")]
        public int BasketId { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public Basket Basket { get; set; }
        public Food Food { get; set; }
        public int Quantity { get; set; }
        public DateTime LastModified { get; set; }
    }
}
