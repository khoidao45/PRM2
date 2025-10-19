using EVStation_basedRentalSystem.Services.UserAPI.Data;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserDbContext _db;

        public UserService(UserDbContext context, IWebHostEnvironment env, UserDbContext db)
        {
            _context = context;
            _env = env;
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetMyselfAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Replace("Bearer ", ""));
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId)) return null;

            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> UpdateProfileImageUrlAsync(string userId, string imageUrl)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.ProfileImageUrl = imageUrl;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Sync user from AuthAPI (called via /api/user/sync)
        /// </summary>
        public async Task<User> SyncUserAsync(UserDto dto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.ID);

            if (existing == null)
            {
                // create new user
                var newUser = new User
                {
                    Id = dto.ID,
                    Email = dto.Email,
                    Username = dto.Name,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
            else
            {
                // update existing user
                existing.Email = dto.Email;
                existing.Username = dto.Name;
                existing.UpdatedAt = DateTime.UtcNow;

                _context.Users.Update(existing);
                await _context.SaveChangesAsync();
                return existing;
            }
        }
        public async Task CreateUserAsync(UserDto userDto)
        {
            // Check if the user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userDto.ID);
            if (existingUser != null)
            {
                // Update if exists
                existingUser.Username = userDto.Name;
                existingUser.Email = userDto.Email;
                existingUser.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                // Create new user entry
                var newUser = new User
                {
                    Id = userDto.ID,
                    Username = userDto.Name,
                    Email = userDto.Email,
                    CreatedAt = DateTime.UtcNow,
                    Role = userDto.Role,
                };
                _context.Users.Add(newUser);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<string> UpdateUserRoleAsync(string userId, string role)
        {
            // Ensure the User row exists
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                // create minimal user record so userId exists in UserAPI
                user = new User
                {
                    Id = userId,
                    Username = userId, // or any placeholder - you might populate better from payload if sent
                    Email = null,
                    Role = role,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                _db.Users.Add(user);
            }
            else
            {
                user.Role = role;
                _db.Users.Update(user);
            }

            // Remove user from all role-specific tables first
            var existingRenter = await _db.Renters.FindAsync(userId);
            if (existingRenter != null) _db.Renters.Remove(existingRenter);

            var existingStaff = await _db.Staffs.FindAsync(userId);
            if (existingStaff != null) _db.Staffs.Remove(existingStaff);

            var existingAdmin = await _db.Admins.FindAsync(userId);
            if (existingAdmin != null) _db.Admins.Remove(existingAdmin);

            // Add to the correct role table
            switch (role)
            {
                case "Renter":
                    _db.Renters.Add(new Renter
                    {
                        Id = userId,
                        FullName = user.Username,
                        PhoneNumber = "", // optional, update later
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    });
                    break;

                case "Staff":
                    _db.Staffs.Add(new Staff
                    {
                        Id = userId,
                        FullName = user.Username,
                        Email = user.Email,
                        PhoneNumber = "",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    });
                    break;

                case "Admin":
                    _db.Admins.Add(new Admin
                    {
                        Id = userId,
                        FullName = user.Username,
                        ContactNumber = "",
                        RoleLevel = "SystemAdmin",
                        CreatedAt = DateTime.UtcNow
                    });
                    break;

                default:
                    // Role "Guest" or unknown — keep only User row, no role-specific table
                    break;
            }

            await _db.SaveChangesAsync();
            return $"User {userId} role updated to {role}.";
        }

    }
}
