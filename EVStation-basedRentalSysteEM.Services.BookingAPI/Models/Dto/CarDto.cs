namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string LicensePlate { get; set; }
        public decimal HourlyRate { get; set; }
        public string Status { get; set; }
        public int StationId { get; set; }
        public string Color { get; set; } = "Red";
        public int SeatCount { get; set; } = 5;
        public bool IsRegistered { get; set; } = true;
    }
}
