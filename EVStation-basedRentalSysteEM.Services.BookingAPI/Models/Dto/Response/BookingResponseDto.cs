namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }            // Booking ID as int
        public string UserId { get; set; }     // AuthAPI user ID
        public string? UserName { get; set; }
        public int CarId { get; set; }
        public string? CarName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
