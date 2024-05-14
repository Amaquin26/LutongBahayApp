using System.ComponentModel.DataAnnotations;

namespace LutongBahayApp.ViewModels
{
    public class RegisterSellerViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string MarketName { get; set; }
        public string MarketAddress { get; set; }
        public string MarketLocation { get; set; }
    }
}
