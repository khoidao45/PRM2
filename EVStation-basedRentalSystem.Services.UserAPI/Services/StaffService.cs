using EVStation_basedRentalSystem.Services.UserAPI.Data;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly UserDbContext _context;
        private readonly IWebHostEnvironment _env;

        public StaffService(UserDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staffs
                .Where(s => s.IsActive)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<Staff?> GetByIdAsync(string id)
        {
            return await _context.Staffs
                .Where(s => s.IsActive)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _context.Staffs
                .Where(s => s.IsActive && s.User.Email == email)
                .Include(s => s.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Staff?> GetMyselfAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Replace("Bearer ", ""));
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId)) return null;

            return await GetByIdAsync(userId);
        }

        public async Task<Staff> CreateAsync(Staff staff)
        {
            staff.IsActive = true;
            staff.CreatedAt = DateTime.UtcNow;
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<Staff?> UpdateAsync(Staff staff)
        {
            var existing = await _context.Staffs.FindAsync(staff.Id);
            if (existing == null) return null;

            existing.FullName = staff.FullName;
            existing.Position = staff.Position;
            existing.Department = staff.Department;
            existing.StationAssigned = staff.StationAssigned;
            existing.WorkShift = staff.WorkShift;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.Staffs.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return false;

            staff.IsActive = false;
            staff.UpdatedAt = DateTime.UtcNow;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Staff>> GetByShiftAsync(string shift)
        {
            return await _context.Staffs
                .Where(s => s.IsActive && s.WorkShift == shift)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<Staff?> AssignShiftAsync(string staffId, string shift)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff == null) return null;

            staff.WorkShift = shift;
            staff.UpdatedAt = DateTime.UtcNow;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<IEnumerable<Staff>> GetByDepartmentAsync(string department)
        {
            return await _context.Staffs
                .Where(s => s.IsActive && s.Department == department)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetByPositionAsync(string position)
        {
            return await _context.Staffs
                .Where(s => s.IsActive && s.Position == position)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<Staff?> AssignPositionAsync(string staffId, string position)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff == null) return null;

            staff.Position = position;
            staff.UpdatedAt = DateTime.UtcNow;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<Staff?> AssignDepartmentAsync(string staffId, string department)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff == null) return null;

            staff.Department = department;
            staff.UpdatedAt = DateTime.UtcNow;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
            return staff;
        }
    }
}
