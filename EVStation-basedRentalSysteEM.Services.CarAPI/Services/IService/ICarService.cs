using System.Collections.Generic;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.CarAPI.Models;

namespace EVStation_basedRentalSystem.Services.CarAPI.Services.IService
{
    public interface ICarService
    {
        // Basic CRUD
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task<Car> AddCarAsync(Car car);
        Task<Car> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(int id);

        // ---- Extended ----

        // Get all cars belonging to a specific station
        Task<IEnumerable<Car>> GetCarsByStationIdAsync(int stationId);

        // Get only available cars
        Task<IEnumerable<Car>> GetAvailableCarsAsync();

        // Change car state (Available, In Use, Maintenance)
        Task<bool> UpdateCarStateAsync(int carId, string newState);

        // Update battery level
        Task<bool> UpdateBatteryLevelAsync(int carId, decimal newBatteryLevel);

        // Filter cars by brand, model, or city
        Task<IEnumerable<Car>> SearchCarsAsync(string keyword);

        // Maintenance utilities
        Task<IEnumerable<Car>> GetCarsNeedingMaintenanceAsync();
    }
}
