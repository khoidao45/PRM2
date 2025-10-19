using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDto> GetAccountAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return await MapToDtoAsync(user);
        }

        public async Task<List<UserDto>> FindAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserDto>();

            foreach (var user in users)
                result.Add(await MapToDtoAsync(user));

            return result;
        }

        public async Task<UserDto> FindByIdAsync(string accountId)
        {
            var user = await _userManager.FindByIdAsync(accountId);
            if (user == null) throw new KeyNotFoundException("User not found.");
            return await MapToDtoAsync(user);
        }

        public async Task<UserDto> ActivateAccountAsync(string verificationCode)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SecurityStamp == verificationCode);
            if (user == null) throw new KeyNotFoundException("Invalid verification code.");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return await MapToDtoAsync(user);
        }

        public async Task<string> DeleteAccountAsync(string accountId)
        {
            var user = await _userManager.FindByIdAsync(accountId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to delete account: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return "Account deleted successfully.";
        }

        private async Task<UserDto> MapToDtoAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role ?? roles.FirstOrDefault() ?? string.Empty,
                ProfileImageUrl = user.ProfileImageUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
