using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models.DTO;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);
        Task<User?> GetMyselfAsync(string token);
        Task CreateUserAsync(UserDto userDto);


        // Optional: profile picture upload
        Task<User?> UpdateProfileImageUrlAsync(string userId, string imageUrl);
    }
}