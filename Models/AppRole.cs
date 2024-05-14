using Microsoft.AspNetCore.Identity;

namespace MVCWebApp.Models
{
    public class AppRole : IdentityRole
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Seller = "Seller";
    }
}
