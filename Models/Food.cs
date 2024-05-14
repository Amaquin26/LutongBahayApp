using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LutongBahayApp.Data.Enum;

namespace LutongBahayApp.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("Market")]
        public int MarketId { get; set; }
        public Market Market { get; set; }
        public decimal Price { get; set; }
        public decimal Sales { get; set; }
        public FoodAvailabilityStatus availabilityStatus { get; set; } = FoodAvailabilityStatus.Available;
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderFood> OrderFood { get; set; }
        public ICollection<BasketFood> BasketFood { get; set; }
    }
}
