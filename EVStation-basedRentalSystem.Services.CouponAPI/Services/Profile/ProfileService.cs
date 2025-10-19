using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.AuthAPI.Data;
using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        // Extract User ID (sub/nameid) from token
        private string? ExtractUserIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            token = token.Replace("Bearer ", "").Trim();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                return jwt.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier ||
                    c.Type == "sub" ||
                    c.Type == "nameid")?.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ApplicationUser?> GetMyProfileAsync(string token)
        {
            var userId = ExtractUserIdFromToken(token);
            if (userId == null) return null;

            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser?> UpdateMyProfileAsync(string token, ApplicationUser updateRequest)
        {
            var userId = ExtractUserIdFromToken(token);
            if (userId == null) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            user.Name = updateRequest.Name ?? user.Name;
            user.PhoneNumber = updateRequest.PhoneNumber ?? user.PhoneNumber;
            user.ProfileImageUrl = updateRequest.ProfileImageUrl ?? user.ProfileImageUrl;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser?> UpdateMyProfileImageAsync(string token, string imageUrl)
        {
            var userId = ExtractUserIdFromToken(token);
            if (userId == null) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            user.ProfileImageUrl = imageUrl;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
