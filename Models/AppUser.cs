using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LutongBahayApp.Models
{
    public class AppUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Address { get; set; }
        public DateTime DateJoined { get; set; }
        public string? GoogleSecretKey { get; set; }
        public string? ProfileImagePath { get; set; }  
        public ICollection<Market> Markets { get; set; }
    }
}
