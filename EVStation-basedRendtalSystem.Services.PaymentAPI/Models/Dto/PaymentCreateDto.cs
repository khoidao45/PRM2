namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO
{
    public class PaymentCreateDto
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
    }
}
