using EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto;
using System;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services.IService
{
    public interface IHopDongService
    {
        Task<Guid> TaoHopDongAsync(TaoHopDongDto request);
        Task GuiEmailXacNhanAsync(Guid hopDongId, string email);
        Task XacNhanHopDongAsync(string token);
        Task<Guid> GetHopDongIdByTokenAsync(string token);
    }
}
