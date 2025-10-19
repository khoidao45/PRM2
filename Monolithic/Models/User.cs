using System.ComponentModel.DataAnnotations;


namespace EVStation_basedRentalSystem.Services.UserAPI.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } 

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }



        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
