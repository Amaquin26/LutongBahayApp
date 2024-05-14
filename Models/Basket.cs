using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AspNetUsers")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<BasketFood> BasketFood { get; set;}
    }
}
