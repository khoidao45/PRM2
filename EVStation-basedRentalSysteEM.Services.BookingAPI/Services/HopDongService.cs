using EVStation_basedRentalSysteEM.Services.BookingAPI.Services.IService;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.BookingAPI.Services;
using EVStation_basedRentalSystem.Services.BookingAPI.Services.IService;

namespace EVStation_basedRentalSysteEM.Services.BookingAPI.Services
{
    public class HopDongService : IHopDongService
    {
        private readonly HttpClient _httpClient;

        public HopDongService(HttpClient httpClient)  // <== HttpClient trực tiếp
        {
            _httpClient = httpClient;
        }

        public async Task<Guid> TaoHopDongAsync(TaoHopDongDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/hopdong/tao-hop-dong", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            return Guid.Parse(result.HopDongId.ToString());
        }

        public async Task GuiEmailXacNhanAsync(Guid hopDongId, string email)
        {
            var emailRequest = new { Email = email };
            var response = await _httpClient.PostAsJsonAsync($"api/hopdong/{hopDongId}/gui-xac-nhan", emailRequest);
            response.EnsureSuccessStatusCode();
        }

        public async Task XacNhanHopDongAsync(string token)
        {
            var response = await _httpClient.PostAsJsonAsync("api/hopdong/xac-nhan", new { Token = token });
            response.EnsureSuccessStatusCode();
        }

        public async Task<Guid> GetHopDongIdByTokenAsync(string token)
        {
            // Giả sử API HopDong có endpoint trả Id theo token
            var response = await _httpClient.GetAsync($"https://localhost:7063/api/hopdong/token/{token}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            return Guid.Parse(result.HopDongId.ToString());
        }
    }
}
