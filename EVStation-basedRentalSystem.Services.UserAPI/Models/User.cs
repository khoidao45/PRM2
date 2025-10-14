using System;
using System.ComponentModel.DataAnnotations;

namespace EVStation_basedRentalSystem.Services.UserAPI.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } // Matches ApplicationUser.Id

        public string Email { get; set; }
        public string Username { get; set; }
        public string? ProfileImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Admin Admin { get; set; }
        public Renter Renter { get; set; }
        public Staff Staff { get; set; }
    }
}
