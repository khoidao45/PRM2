using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto.Request;
using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto.Response;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
