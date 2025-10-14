using EVStation_basedRentalSystem.Services.UserAPI.Models;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IAdminService
    {
        Task<IEnumerable<Admin>> GetAllAsync();
        Task<Admin?> GetByIdAsync(string id);
        Task<Admin?> GetByEmailAsync(string email);
        Task<Admin> CreateAsync(Admin admin);
        Task<Admin?> UpdateAsync(Admin admin);
        Task<bool> DeleteAsync(string id);
        Task<Admin?> GetMyselfAsync(string token);
    }
}
