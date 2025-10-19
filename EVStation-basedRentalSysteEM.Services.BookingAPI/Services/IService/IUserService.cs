using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto;
using System.Threading.Tasks;

namespace EVStation_basedRentalSysteEM.Services.BookingAPI.Services.IService
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(string userId);
    }
}
