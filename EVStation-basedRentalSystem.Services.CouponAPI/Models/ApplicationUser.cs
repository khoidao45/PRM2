using Microsoft.AspNetCore.Identity;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
