using LutongBahayApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.ViewModels
{
    public class FoodItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Market Market { get; set; }  
        public int MarketTotalReviews { get; set; }
        public decimal Price { get; set; }

        public double Rating { get; set; } = 0;
        public bool HasReviewed { get; set; } = false;     
        public List<Review> Reviews { get; set; }
        public Review UserReview { get; set; }
        public bool CanReview { get; set; }
    }
}
