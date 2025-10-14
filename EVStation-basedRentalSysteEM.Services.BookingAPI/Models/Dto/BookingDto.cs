namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO
{
    public class BookingDTO
    {
        public string UserId { get; set; }      // The user who books
        public int CarId { get; set; }       // The car being booked
        public DateTime StartDate { get; set; }  // When booking starts
        public DateTime EndDate { get; set; }    // When booking ends
    }
}