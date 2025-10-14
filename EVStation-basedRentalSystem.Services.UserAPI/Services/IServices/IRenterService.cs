using EVStation_basedRentalSystem.Services.UserAPI.Models;
using Microsoft.AspNetCore.Http;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IRenterService
    {
        Task<IEnumerable<Renter>> GetAllAsync();
        Task<Renter?> GetByIdAsync(string id);
        Task<Renter?> GetByEmailAsync(string email);
        Task<Renter> CreateAsync(Renter renter);
        Task<Renter?> UpdateAsync(Renter renter);
        Task<Renter?> GetMyselfAsync(string token);

        // File uploads specific to renter
        Task<Renter?> UpdateDriverLicenseUrlAsync(string renterId, string imageUrl);
        Task<Renter?> UpdateIdentityCardUrlAsync(string renterId, string imageUrl);

    }
}
