using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Clients
{
    public class UserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/auth/{userId}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserDto>();
            return null;
        }
    }
}
