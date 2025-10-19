using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models;

namespace EVStation_basedRentalSystem.Services.UserAPI.Services.IService
{
    public interface IStaffProfileService
    {
        Task<IEnumerable<StaffProfile>> GetAllAsync();
        Task<StaffProfile?> GetByIdAsync(string id);
        Task<StaffProfile> CreateAsync(StaffProfile profile);
        Task<StaffProfile?> UpdateAsync(StaffProfile profile);
        Task<bool> DeleteAsync(string id);

        // Role-specific logic
        Task<StaffProfile?> AssignShiftAsync(string staffId, string shift);
        Task<StaffProfile?> AssignDepartmentAsync(string staffId, string department);
    }
}
