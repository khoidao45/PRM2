using System;
using System.ComponentModel.DataAnnotations;

namespace EVStation_basedRentalSystem.Services.IncidentAPI.Models
{
    public class Incident
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }  // Links to the contract

        [Required]
        public int CarId { get; set; }       // Car involved in the incident

        [Required]
        public int UserId { get; set; }      // Customer who reported the incident

        // Optional staff references
        
        public int StaffId { get; set; }  // Staff who resolves the incident

        [Required]
        [MaxLength(100)]
        public string Type { get; set; }     // e.g., "LateReturn", "Damage", "IssueReport"

        [Required]
        public DateTime OccurredAt { get; set; } // When the incident happened

        [MaxLength(1000)]
        public string Description { get; set; }  // Optional details

        [Range(0, double.MaxValue)]
        public decimal PenaltyAmount { get; set; } // Any fee due to the incident

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Resolved, Paid

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }       // Optional: when it was resolved
    }
}
