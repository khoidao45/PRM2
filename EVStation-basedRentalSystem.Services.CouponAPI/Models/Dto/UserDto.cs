namespace EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto
{
    public class UserDto
    {
        public string ID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
