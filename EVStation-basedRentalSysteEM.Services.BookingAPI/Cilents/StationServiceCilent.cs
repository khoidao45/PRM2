
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;

    namespace EVStation_basedRentalSystem.Services.BookingAPI.Clients
    {
        public class StationServiceClient
        {
            private readonly HttpClient _httpClient;

            public StationServiceClient(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<StationDto?> GetStationByIdAsync(int stationId)
            {
                var response = await _httpClient.GetAsync($"/api/station/{stationId}");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<StationDto>();
                return null;
            }
        }
    
}
