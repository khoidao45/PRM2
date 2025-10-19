using EVStation_basedRentalSystem.Services.BookingAPI.Data;
using EVStation_basedRentalSystem.Services.BookingAPI.Models;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Services.IService;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext _context;
        private readonly IHopDongService _hopDongService;
        private readonly IUserService _userService;
        private readonly ICarService _carService;

        public BookingService(
            BookingDbContext context,
            IHopDongService hopDongService,
            IUserService userService,
            ICarService carService)
        {
            _context = context;
            _hopDongService = hopDongService;
            _userService = userService;
            _carService = carService;
        }

        // ----------------------------
        // Map BookingDTO -> TaoHopDongDto
        // ----------------------------
        private TaoHopDongDto MapToHopDongDto(BookingDTO bookingDto, UserDto user, CarDto car, decimal totalPrice)
        {
            int days = (bookingDto.EndDate - bookingDto.StartDate).Days;
            days = days <= 0 ? 1 : days;
            string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            return new TaoHopDongDto(
                SoHopDong: $"HD-{Guid.NewGuid().ToString().Substring(0, 8)}",
                NgayKy: DateTime.UtcNow.Day.ToString(),
                ThangKy: DateTime.UtcNow.Month.ToString(),
                NamKy: DateTime.UtcNow.Year.ToString(),
                BenA: new ThongTinBenA(
                    HoTen: user.FullName,
                    NamSinh: user.BirthDate ?? "1990-01-01",
                    CccdHoacHoChieu: user.IdCard ?? "123456789",
                    HoKhauThuongTru: user.Address ?? "Ha Noi"
                ),
                Xe: new ThongTinXe(
                    NhanHieu: car.Brand,
                    BienSo: car.LicensePlate,
                    LoaiXe: car.Model,
                    MauSon: car.Color ?? "Red",
                    ChoNgoi: car.SeatCount.ToString(),
                    XeDangKiHan: car.IsRegistered ? "Yes" : "No"
                ),
                GiaThue: new ThongTinGiaThue(
                    GiaThueSo: totalPrice.ToString("F2"),
                    GiaThueChu: totalPrice.ToString("C"),
                    PhuongThucThanhToan: "Tiền mặt",
                    NgayThanhToan: today
                ),
                ThoiHanThueSo: days.ToString(),
                ThoiHanThueChu: $"{days} ngày",
                ThoiHanThue: days,
                DonViThoiHan: "ngay",
                GPLX: new ThongTinGPLX(
                    Hang: user.DriverLicenseClass ?? "A",
                    So: user.DriverLicenseNumber ?? "123456",
                    HanSuDung: user.DriverLicenseExpiry ?? "2030-12-31"
                )
            );
        }

        // ----------------------------
        // 1️⃣ Get all bookings
        // ----------------------------
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() =>
            await _context.Bookings.ToListAsync();

        // ----------------------------
        // 2️⃣ Get booking by Id
        // ----------------------------
        public async Task<Booking?> GetBookingByIdAsync(int id) =>
            await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

        // ----------------------------
        // 3️⃣ Create booking + HopDong
        // ----------------------------
        public async Task<Booking> CreateBookingAsync(BookingDTO bookingDto)
        {
            var user = await _userService.GetUserByIdAsync(bookingDto.UserId)
                ?? throw new Exception("User not found");

            var car = await _carService.GetCarByIdAsync(bookingDto.CarId)
                ?? throw new Exception("Car not found");

            double hours = (bookingDto.EndDate - bookingDto.StartDate).TotalHours;
            if (hours <= 0) throw new Exception("EndDate must be after StartDate");

            decimal totalPrice = Math.Round((decimal)hours * car.HourlyRate, 2);
            var hopDongDto = MapToHopDongDto(bookingDto, user, car, totalPrice);

            // Tạo HopDong bên ngoài
            var hopDongId = await _hopDongService.TaoHopDongAsync(hopDongDto);
            await _hopDongService.GuiEmailXacNhanAsync(hopDongId, user.Email);

            // Tạo booking trong DB
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                CarId = bookingDto.CarId,
                StationId = car.StationId,
                HopDongId = hopDongId,
                StartTime = bookingDto.StartDate,
                EndTime = bookingDto.EndDate,
                TotalPrice = totalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        // ----------------------------
        // 4️⃣ Confirm booking HopDong
        // ----------------------------
        public async Task<Booking?> ConfirmBookingHopDongAsync(string token)
        {
            await _hopDongService.XacNhanHopDongAsync(token);
            var hopDongId = await _hopDongService.GetHopDongIdByTokenAsync(token);

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.HopDongId == hopDongId);
            if (booking != null)
            {
                booking.Status = "Confirmed";
                booking.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return booking;
        }

        // ----------------------------
        // 5️⃣ Update booking status (Pending → Confirmed/Cancelled/Completed)
        // ----------------------------
        public async Task<Booking?> UpdateBookingStatusAsync(int id, string newStatus)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return null;

            // Optional: validate allowed status transitions
            var allowed = new List<string> { "Pending", "Confirmed", "Cancelled", "Completed" };
            if (!allowed.Contains(newStatus))
                throw new Exception($"Invalid status: {newStatus}");

            booking.Status = newStatus;
            booking.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return booking;
        }

        // ----------------------------
        // 6️⃣ Cancel booking
        // ----------------------------
        public async Task<bool> CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return false;

            booking.Status = "Cancelled"; // hủy thay vì remove để lịch sử
            booking.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
