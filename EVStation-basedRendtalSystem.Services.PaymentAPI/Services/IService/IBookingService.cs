using EVStation_basedRendtalSystem.Services.PaymentAPI.Models.Dto;

namespace EVStation_basedRendtalSystem.Services.PaymentAPI.Services.IService
{
    public interface IBookingService
    {
        Task<BookingDto?> GetBookingByIdAsync(int bookingId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);
    }
}
