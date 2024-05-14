using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class SoldFoodMetric
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public DateTime DateSold { get; set; }
        public int SoldCount { get; set; }
    }
}
