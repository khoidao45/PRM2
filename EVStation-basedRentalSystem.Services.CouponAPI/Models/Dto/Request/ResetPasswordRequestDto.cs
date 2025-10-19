namespace EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto.Request
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;   // <-- token from email
        public string NewPassword { get; set; } = null!;
    }
}
