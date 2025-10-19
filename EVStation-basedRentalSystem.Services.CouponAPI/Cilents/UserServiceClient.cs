using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Clients
{
    public class UserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateUserAsync(object user)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/user/sync", user);
            return response.IsSuccessStatusCode;
        }

        // New: notify UserAPI of a role change
        public async Task<bool> SyncUserRoleAsync(string userId, string role)
        {
            var payload = new
            {
                UserId = userId,
                Role = role
            };

            var response = await _httpClient.PostAsJsonAsync("/api/user/sync-role", payload);
            return response.IsSuccessStatusCode;
        }
    }
}
