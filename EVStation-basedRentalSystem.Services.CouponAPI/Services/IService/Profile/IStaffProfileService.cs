using EVStation_basedRentalSystem.Services.AuthAPI.Models;

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

    // Approve renter
    Task<bool> ApproveRenterAsync(string renterId);

    // Booking management
 
}
