using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
    }
}
