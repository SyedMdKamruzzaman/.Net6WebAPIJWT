using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Mobile { get; set; } = String.Empty;
        public string Company { get; set; } = String.Empty;
    }
}
