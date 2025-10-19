using EVStation_basedRentalSystem.Services.BookingAPI.Data;
using EVStation_basedRentalSystem.Services.BookingAPI.Models;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.BookingAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Services.IService;
using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services
{
    // ----------------------------
    // Booking Service
    // ----------------------------
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
        // 1️⃣ Map BookingDTO -> HopDongDTO
        // ----------------------------
        private TaoHopDongDto MapToHopDongDto(BookingDTO bookingDto, UserDto user, CarDto car, decimal totalPrice)
        {
            return new TaoHopDongDto(
                SoHopDong: $"HD-{Guid.NewGuid().ToString().Substring(0, 8)}",
                NgayKy: DateTime.UtcNow.Day.ToString(),
                ThangKy: DateTime.UtcNow.Month.ToString(),
                NamKy: DateTime.UtcNow.Year.ToString(),
                BenA: new ThongTinBenA(user.FullName, "", "", ""),
                Xe: new ThongTinXe(car.Brand, car.LicensePlate, car.Model, "", "", ""),
                GiaThue: new ThongTinGiaThue(totalPrice.ToString(), "", "Tiền mặt", DateTime.UtcNow.ToString("dd/MM/yyyy")),
                ThoiHanThueSo: (bookingDto.EndDate - bookingDto.StartDate).Days.ToString(),
                ThoiHanThueChu: "",
                ThoiHanThue: (bookingDto.EndDate - bookingDto.StartDate).Days,
                DonViThoiHan: "ngay",
                GPLX: new ThongTinGPLX("", "", "")
            );
        }

        // ----------------------------
        // 2️⃣ Get all bookings
        // ----------------------------
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        // ----------------------------
        // 3️⃣ Get booking by Id
        // ----------------------------
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        }

        // ----------------------------
        // 4️⃣ Create Booking + HopDong
        // ----------------------------
        public async Task<Booking> CreateBookingAsync(BookingDTO bookingDto)
        {
            var user = await _userService.GetUserByIdAsync(bookingDto.UserId);
            if (user == null) throw new Exception("User not found");

            var car = await _carService.GetCarByIdAsync(bookingDto.CarId);
            if (car == null) throw new Exception("Car not found");

            double hours = (bookingDto.EndDate - bookingDto.StartDate).TotalHours;
            if (hours <= 0) throw new Exception("EndDate must be after StartDate");

            decimal totalPrice = Math.Round((decimal)hours * car.HourlyRate, 2);

            var hopDongDto = MapToHopDongDto(bookingDto, user, car, totalPrice);

            // --------------- Gọi HopDong Service ---------------
            var hopDongId = await _hopDongService.TaoHopDongAsync(hopDongDto);

            await _hopDongService.GuiEmailXacNhanAsync(hopDongId, user.Email);

            // --------------- Tạo booking trong DB ---------------
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
        // 5️⃣ Confirm Booking sau khi HopDong được xác nhận
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
        // 6️⃣ Update status
        // ----------------------------
        public async Task<Booking?> UpdateBookingStatusAsync(int id, string newStatus)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return null;

            booking.Status = newStatus;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return booking;
        }

        // ----------------------------
        // 7️⃣ Cancel booking
        // ----------------------------
        public async Task<bool> CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }
    }

    // ----------------------------
    // DTO HopDong và sub-DTO gói gọn bên trong BookingAPI
    // ----------------------------
    public record TaoHopDongDto(
        string SoHopDong,
        string NgayKy,
        string ThangKy,
        string NamKy,
        ThongTinBenA BenA,
        ThongTinXe Xe,
        ThongTinGiaThue GiaThue,
        string ThoiHanThueSo,
        string ThoiHanThueChu,
        int ThoiHanThue,
        string DonViThoiHan,
        ThongTinGPLX GPLX
    );

    public record ThongTinBenA(string HoTen, string NamSinh, string CccdHoacHoChieu, string HoKhauThuongTru);
    public record ThongTinXe(string NhanHieu, string BienSo, string LoaiXe, string MauSon, string ChoNgoi, string XeDangKiHan);
    public record ThongTinGiaThue(string GiaThueSo, string GiaThueChu, string PhuongThucThanhToan, string NgayThanhToan);
    public record ThongTinGPLX(string Hang, string So, string HanSuDung);
}
