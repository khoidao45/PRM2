using System.Collections.Generic;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.StationAPI.Models;

namespace EVStation_basedRentalSystem.Services.StationAPI.Services.IService
{
    public interface IStationService
    {
        // Basic CRUD
        Task<IEnumerable<Station>> GetAllStationsAsync();
        Task<Station> GetStationByIdAsync(int id);
        Task<Station> AddStationAsync(Station station);
        Task<Station> UpdateStationAsync(Station station);
        Task<bool> DeleteStationAsync(int id);

        // ---- Extended ----

        // Get only active stations
        Task<IEnumerable<Station>> GetActiveStationsAsync();

        // Search by city or name
        Task<IEnumerable<Station>> SearchStationsAsync(string keyword);

        // Update status (Active / Inactive / Maintenance)
        Task<bool> UpdateStationStatusAsync(int stationId, string status);

        // Get stations that have available cars (via API call to Car service)
        Task<IEnumerable<Station>> GetStationsWithAvailableCarsAsync();
    }
}
