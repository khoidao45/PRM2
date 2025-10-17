namespace EVStation_basedRendtalSystem.Services.PaymentAPI.Models.Dto
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public int CarId { get; set; }
        public int StationId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
