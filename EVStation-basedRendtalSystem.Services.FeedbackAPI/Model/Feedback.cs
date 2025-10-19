using System.ComponentModel.DataAnnotations;

namespace EVStation_basedRentalSystem.Services.RatingAPI.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }   // who rated

        [Required]
        public int CarId { get; set; }    // which car was rated

        [Required]
        [Range(1, 5)]
        public int Score { get; set; }    // renamed from 'rating' column

        [MaxLength(500)]
        public string Feedback { get; set; }

        // Optional relationships
        // public User User { get; set; }
        // public Car Car { get; set; }
    }
}
