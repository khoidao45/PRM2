using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EVStation_basedRentalSystem.Services.UserAPI.Clients
{
    public class AuthServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["AuthService:BaseUrl"]; // ✅ Read from appsettings.json
        }

        // Example: validate a token
        public async Task<bool> ValidateTokenAsync(string token)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/auth/validate?token={token}");
            return response.IsSuccessStatusCode;
        }
    }
}
