using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto.Request;
using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto.Response;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Service.IService
{
    public interface IRegistrationService
    {
        Task<RegistrationResponseDto> RegisterUserAsync(RegistrationRequestDto registerRequest);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<string> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequest);
    }
}
