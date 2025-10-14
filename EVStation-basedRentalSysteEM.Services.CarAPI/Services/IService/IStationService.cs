using EVStation_basedRentalSystem.Services.CarAPI.Models.DTO;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.CarAPI.Services.IService
{
    public interface IStationService
    {
        Task<StationDto?> GetStationByIdAsync(int stationId);

    }
}
