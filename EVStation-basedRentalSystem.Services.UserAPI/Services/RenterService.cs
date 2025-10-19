using EVStation_basedRentalSystem.Services.UserAPI.Data;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services
{
    public class RenterService : IRenterService
    {
        private readonly UserDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RenterService(UserDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Renter>> GetAllAsync()
        {
            return await _context.Renters
                .Where(r => r.IsActive)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Renter?> GetByIdAsync(string id)
        {
            return await _context.Renters
                .Where(r => r.IsActive)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Renter?> GetByEmailAsync(string email)
        {
            return await _context.Renters
                .Where(r => r.IsActive && r.User.Email == email)
                .Include(r => r.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Renter?> GetMyselfAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Replace("Bearer ", ""));
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId)) return null;

            return await GetByIdAsync(userId);
        }

        public async Task<Renter> CreateAsync(Renter renter)
        {
            renter.IsActive = true;
            renter.CreatedAt = DateTime.UtcNow;
            _context.Renters.Add(renter);
            await _context.SaveChangesAsync();
            return renter;
        }

        public async Task<Renter?> UpdateAsync(Renter renter)
        {
            var existing = await _context.Renters.FindAsync(renter.Id);
            if (existing == null) return null;

            existing.FullName = renter.FullName;
            existing.PhoneNumber = renter.PhoneNumber;
            existing.Address = renter.Address;
            existing.DriverLicenseNumber = renter.DriverLicenseNumber;
            existing.IdentityCardNumber = renter.IdentityCardNumber;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.Renters.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }

        public async Task<Renter?> UpdateDriverLicenseUrlAsync(string renterId, string imageUrl)
        {
            var renter = await _context.Renters.FindAsync(renterId);
            if (renter == null) return null;

            renter.DriverLicenseImageUrl = imageUrl;
            renter.UpdatedAt = DateTime.UtcNow;

            _context.Renters.Update(renter);
            await _context.SaveChangesAsync();

            return renter;
        }


        public async Task<Renter?> UpdateIdentityCardUrlAsync(string renterId, string imageUrl)
        {
            var renter = await _context.Renters.FindAsync(renterId);
            if (renter == null) return null;

            renter.IdentityCardImageUrl = imageUrl;
            renter.UpdatedAt = DateTime.UtcNow;

            _context.Renters.Update(renter);
            await _context.SaveChangesAsync();

            return renter;
        }
    }
}
