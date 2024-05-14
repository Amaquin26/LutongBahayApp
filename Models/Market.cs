using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LutongBahayApp.Models
{
    public class Market
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsVerified { get; set; }
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<Food> Foods { get; set; }

    }
}
