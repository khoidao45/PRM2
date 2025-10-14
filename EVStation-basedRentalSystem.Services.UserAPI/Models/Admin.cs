using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVStation_basedRentalSystem.Services.UserAPI.Models
{
    public class Admin
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; } // Same as User.Id

        public string FullName { get; set; }
        public string RoleLevel { get; set; } // SuperAdmin, SystemAdmin, StationAdmin
        public string ManagedStation { get; set; }
        public string ContactNumber { get; set; }

        public bool CanApproveUsers { get; set; } = true;
        public bool CanManageStaff { get; set; } = true;
        public bool CanViewReports { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
    }
}
