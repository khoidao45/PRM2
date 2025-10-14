using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVStation_basedRentalSystem.Services.UserAPI.Models
{
    public class Staff
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; } // Same as User.Id

        public string FullName { get; set; }
        public string Position { get; set; }
        public string StationAssigned { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string WorkShift { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
    }
}
