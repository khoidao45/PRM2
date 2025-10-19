using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDto userDto);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);
        Task<User> GetMyselfAsync(string token);
        Task<User> UpdateProfileImageUrlAsync(string id, string imageUrl);

        // NEW: update role (sync)
        Task<string> UpdateUserRoleAsync(string userId, string role);
    }
}
