using EVStation_basedRentalSystem.Services.AuthAPI.Data;
using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.Profile
{
    public class RenterProfileService : IRenterProfileService
    {
        private readonly AppDbContext _context;

        public RenterProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RenterProfile>> GetAllAsync()
        {
            return await _context.RenterProfiles.ToListAsync();
        }

        public async Task<RenterProfile?> GetByIdAsync(string id)
        {
            return await _context.RenterProfiles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RenterProfile> CreateAsync(RenterProfile profile)
        {
            _context.RenterProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<RenterProfile?> UpdateAsync(RenterProfile profile)
        {
            var existing = await _context.RenterProfiles.FirstOrDefaultAsync(r => r.Id == profile.Id);
            if (existing == null) return null;

            existing.FullName = profile.FullName;
            existing.PhoneNumber = profile.PhoneNumber;
            existing.Address = profile.Address;
            existing.Gender = profile.Gender;
            existing.DateOfBirth = profile.DateOfBirth;
            existing.EmergencyContactName = profile.EmergencyContactName;
            existing.EmergencyContactPhone = profile.EmergencyContactPhone;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.RenterProfiles.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var renter = await _context.RenterProfiles.FirstOrDefaultAsync(r => r.Id == id);
            if (renter == null) return false;

            _context.RenterProfiles.Remove(renter);
            await _context.SaveChangesAsync();
            return true;
        }

       

        // -------------------------------
        // Renter-specific actions
        // -------------------------------
        public async Task<RenterProfile?> UpdateDriverLicenseAsync(string renterId, string licenseNumber, string imageUrl)
        {
            var renter = await _context.RenterProfiles.FirstOrDefaultAsync(r => r.Id == renterId);
            if (renter == null) return null;

            renter.DriverLicenseNumber = licenseNumber;
            renter.DriverLicenseImageUrl = imageUrl;
            renter.UpdatedAt = DateTime.UtcNow;

            _context.RenterProfiles.Update(renter);
            await _context.SaveChangesAsync();
            return renter;
        }

        public async Task<RenterProfile?> UpdateIdentityCardAsync(string renterId, string cardNumber, string imageUrl)
        {
            var renter = await _context.RenterProfiles.FirstOrDefaultAsync(r => r.Id == renterId);
            if (renter == null) return null;

            renter.IdentityCardNumber = cardNumber;
            renter.IdentityCardImageUrl = imageUrl;
            renter.UpdatedAt = DateTime.UtcNow;

            _context.RenterProfiles.Update(renter);
            await _context.SaveChangesAsync();
            return renter;
        }
    }
    }
