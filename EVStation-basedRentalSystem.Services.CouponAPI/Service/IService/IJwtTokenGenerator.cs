using EVStation_basedRentalSystem.Services.AuthAPI.Models;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
