using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVStation_basedRentalSystem.Services.UserAPI.Models
{
    public class Renter
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; } // Same as User.Id

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseImageUrl { get; set; }
        public string IdentityCardNumber { get; set; }
        public string IdentityCardImageUrl { get; set; }

        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
    }
}
