using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Location { get; set; }
        public int Rating { get; set; }
        public int HelpfulCount { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
