using System.ComponentModel.DataAnnotations;

namespace EVStation_basedRentalSystem.Services.StationAPI.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(30)]
        public string Status { get; set; }  // e.g., "Active", "Inactive", "Under Maintenance"

        // Optional relationships
        // public ICollection<Car> Cars { get; set; }
        // public ICollection<Rating> Ratings { get; set; }
    }
}
