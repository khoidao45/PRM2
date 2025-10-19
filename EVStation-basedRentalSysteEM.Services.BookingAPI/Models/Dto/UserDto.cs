namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string? BirthDate { get; set; }
        public string? IdCard { get; set; }
        public string? Address { get; set; }
        public string? DriverLicenseClass { get; set; }
        public string? DriverLicenseNumber { get; set; }
        public string? DriverLicenseExpiry { get; set; }
    }
}
