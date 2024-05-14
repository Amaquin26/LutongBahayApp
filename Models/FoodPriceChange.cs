using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LutongBahayApp.Models
{
    public class FoodPriceChange
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public DateTime DateChanged { get; set; }
        public decimal Price { get; set; }
    }
}
