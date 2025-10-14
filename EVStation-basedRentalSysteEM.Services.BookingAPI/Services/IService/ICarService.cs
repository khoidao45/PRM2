using System.Threading.Tasks;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;

namespace EVStation_basedRentalSysteEM.Services.BookingAPI.Services.IService
{
    public interface ICarService
    {
        Task<CarDto?> GetCarByIdAsync(int carId);
    }
}
