using EVStation_basedRentalSystem.Services.UserAPI.Models;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllAsync();
        Task<Staff?> GetByIdAsync(string id);
        Task<Staff?> GetByEmailAsync(string email);
        Task<Staff> CreateAsync(Staff staff);
        Task<Staff?> UpdateAsync(Staff staff);
        Task<bool> DeleteAsync(string id);
        Task<Staff?> GetMyselfAsync(string token);
        Task<IEnumerable<Staff>> GetByDepartmentAsync(string department);
        Task<IEnumerable<Staff>> GetByPositionAsync(string position);
        Task<Staff?> AssignPositionAsync(string staffId, string position);
        Task<Staff?> AssignDepartmentAsync(string staffId, string department);
        Task<IEnumerable<Staff>> GetByShiftAsync(string shift);
        Task<Staff?> AssignShiftAsync(string staffId, string shift);
    }
}
