using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services
{
    public interface IHopDongService
    {
        Task<Guid> TaoHopDongAsync(TaoHopDongDto request);
        Task GuiEmailXacNhanAsync(Guid hopDongId, string email);
        Task XacNhanHopDongAsync(string token);
        Task<Guid> GetHopDongIdByTokenAsync(string token);
    }
}