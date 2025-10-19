using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IRenterProfileService
    {
        Task<IEnumerable<RenterProfile>> GetAllAsync();
        Task<RenterProfile?> GetByIdAsync(string id);
        Task<RenterProfile> CreateAsync(RenterProfile profile);
        Task<RenterProfile?> UpdateAsync(RenterProfile profile);
        Task<bool> DeleteAsync(string id);

        // Renter-specific actions
        Task<RenterProfile?> UpdateDriverLicenseAsync(string renterId, string licenseNumber, string imageUrl);
        Task<RenterProfile?> UpdateIdentityCardAsync(string renterId, string cardNumber, string imageUrl);
    }
}
