using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.CarAPI.Data;
using EVStation_basedRentalSystem.Services.CarAPI.Models;
using EVStation_basedRentalSystem.Services.CarAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.CarAPI.Services
{
    public class CarService : ICarService
    {
        private readonly CarDbContext _context;

        public CarService(CarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<Car> UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return false;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Car>> GetCarsByStationIdAsync(int stationId)
        {
            return await _context.Cars
                .Where(c => c.StationId == stationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _context.Cars
                .Where(c => c.State == "Available")
                .ToListAsync();
        }

        public async Task<bool> UpdateCarStateAsync(int carId, string newState)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null) return false;

            car.State = newState;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBatteryLevelAsync(int carId, decimal newBatteryLevel)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null) return false;

            car.CurrentBatteryLevel = newBatteryLevel;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(string keyword)
        {
            return await _context.Cars
                .Where(c => c.Brand.Contains(keyword) || c.Model.Contains(keyword) || c.Color.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsNeedingMaintenanceAsync()
        {
            return await _context.Cars
                .Where(c => c.State == "Maintenance")
                .ToListAsync();
        }
       
        
    }
}
