using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.BookingAPI.Models; // DTO bạn tạo

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services
{
    public class HopDongServiceClient
    {
        private readonly HttpClient _http;

        public HopDongServiceClient(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<Guid> TaoHopDongAsync(TaoHopDongDto request)
        {
            var url = "https://localhost:7063/api/hopdong/taohopdong"; // URL HopDong API

            var response = await _http.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HopDong API Error {response.StatusCode}: {content}");
            }

            var result = await response.Content.ReadFromJsonAsync<HopDongCreateResponse>();
            if (result == null) throw new Exception("HopDong API trả về null");

            return result.Id;
        }
    }

    public record HopDongCreateResponse(Guid Id);
}
