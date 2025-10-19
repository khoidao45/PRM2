namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO
{
    public class BookingDTO
    {
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}