namespace EVStation_basedRentalSystem.Services.CarAPI.Models.DTO
{
    public class CarDto
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string LicensePlate { get; set; }
        public int Seat { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public decimal BatteryCapacity { get; set; }
        public decimal CurrentBatteryLevel { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public DateTime? LastMaintenanceDay { get; set; }
        public string ImageUrl { get; set; }
        public string State { get; set; }
        public string Status { get; set; }

        // Will be filled by StationAPI call
        public StationDto? Station { get; set; }
    }
}
