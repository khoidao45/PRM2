namespace EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string LicensePlate { get; set; }
        public decimal HourlyRate { get; set; }   // for price calculation
        public string Status { get; set; }
        public int StationId { get; set; }
    }

}
