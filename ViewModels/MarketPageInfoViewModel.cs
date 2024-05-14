using LutongBahayApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LutongBahayApp.ViewModels
{
    public class MarketPageInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }
        public int? FoodId { get; set; } = 0;
        public string? FoodName { get; set; } = null;
        public bool IsVerified { get;set; }
        public int TotalReviews { get; set; }
        public int TotalProducts { get; set; }
        public List<FoodListItemViewModel> Foods { get; set; }
    }
}
