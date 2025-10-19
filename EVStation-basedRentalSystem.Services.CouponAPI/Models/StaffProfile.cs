using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Models
{
    public class StaffProfile
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; } // SAME as ApplicationUser.Id

        [Required]
        public string FullName { get; set; }
        public string? Position { get; set; }
        public string? StationAssigned { get; set; }
        public string? Department { get; set; }
        public string? WorkShift { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ApplicationUser User { get; set; }
    }
}
