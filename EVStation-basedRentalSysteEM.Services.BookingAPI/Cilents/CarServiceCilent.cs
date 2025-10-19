
    using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto;
namespace EVStation_basedRentalSystem.Services.BookingAPI.Clients
    {
        public class CarServiceClient
        {
            private readonly HttpClient _httpClient;

            public CarServiceClient(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<CarDto?> GetCarByIdAsync(int carId)
            {
                var response = await _httpClient.GetAsync($"/api/Car/{carId}");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<CarDto>();
                return null;
            }
        }
    }


