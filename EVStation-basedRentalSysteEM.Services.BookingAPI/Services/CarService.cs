using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Services.IService;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services
{
    public class CarService : ICarService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CarService> _logger;

        public CarService(HttpClient httpClient, ILogger<CarService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CarDto?> GetCarByIdAsync(int carId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CarDto>($"api/cars/{carId}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching car with ID {CarId}", carId);
                return null;
            }
        }
    }
}
