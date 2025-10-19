using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.AuthAPI.Models.Dto.Request;
using EVStation_basedRentalSystem.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.AuthAPI.Clients;
using EVStation_basedRentalSystem.Services.AuthAPI.Data;
using EVStation_basedRentalSystem.Services.AuthAPI.Services.IService;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Service
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserServiceClient _userServiceClient;
        private readonly AppDbContext _db;

        public AuthorizeService(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                UserServiceClient userServiceClient,
                                AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userServiceClient = userServiceClient;
            _db = db;
        }

        public async Task<string> AssignRoleAsync(AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) throw new KeyNotFoundException($"User with ID '{request.UserId}' not found.");

            if (!await _roleManager.RoleExistsAsync(request.Role))
                throw new KeyNotFoundException($"Role '{request.Role}' does not exist.");

            var existingRoles = await _userManager.GetRolesAsync(user);
            if (existingRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, existingRoles);
            }

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to assign role '{request.Role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // update local Role field and UpdatedAt
            user.Role = request.Role;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // create profile if role requires (example: Renter)
            if (request.Role == "Renter")
            {
                var r = await _db.RenterProfiles.FindAsync(user.Id);
                if (r == null)
                {
                    _db.RenterProfiles.Add(new RenterProfile { Id = user.Id, FullName = user.Name, PhoneNumber = user.PhoneNumber, CreatedAt = DateTime.UtcNow });
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                // if not renter, remove renter profile if exists (optional)
                var r = await _db.RenterProfiles.FindAsync(user.Id);
                if (r != null)
                {
                    _db.RenterProfiles.Remove(r);
                    await _db.SaveChangesAsync();
                }
            }

            // optional: notify an external UserAPI
            try
            {
                await _userServiceClient.SyncUserRoleAsync(user.Id, request.Role);
            }
            catch
            {
                // log or ignore
            }

            return $"Role '{request.Role}' assigned to user '{user.Email}' successfully.";
        }

        public async Task<string> RevokeRoleAsync(AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) throw new KeyNotFoundException($"User with ID '{request.UserId}' not found.");

            if (!await _roleManager.RoleExistsAsync(request.Role))
                throw new KeyNotFoundException($"Role '{request.Role}' does not exist.");

            if (!await _userManager.IsInRoleAsync(user, request.Role))
                return $"User does not have the role '{request.Role}'.";

            var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to revoke role '{request.Role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // set fallback role / update field
            user.Role = "Guest";
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // remove profile if existed
            var r = await _db.RenterProfiles.FindAsync(user.Id);
            if (r != null)
            {
                _db.RenterProfiles.Remove(r);
                await _db.SaveChangesAsync();
            }

            try
            {
                await _userServiceClient.SyncUserRoleAsync(user.Id, "Guest");
            }
            catch { }

            return $"Role '{request.Role}' revoked from user '{user.Email}' successfully.";
        }
    }
}
