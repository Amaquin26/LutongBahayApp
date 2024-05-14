using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LutongBahayApp.Models
{
    public class SoldMarketMetrics
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Market")]
        public int MarketId { get; set; }
        public Market Market { get; set; }
        public DateTime DateSold { get; set; }
        public int SoldCount { get; set; }
        public decimal Sales { get; set; }
    }
}
