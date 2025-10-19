using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IProfileService
    {
        Task<ApplicationUser?> GetMyProfileAsync(string token);
        Task<ApplicationUser?> UpdateMyProfileAsync(string token, ApplicationUser updateRequest);
        Task<ApplicationUser?> UpdateMyProfileImageAsync(string token, string imageUrl);
    }
}
