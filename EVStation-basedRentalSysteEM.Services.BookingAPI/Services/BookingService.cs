using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Data;
using EVStation_basedRentalSystem.Services.BookingAPI.Models;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.BookingAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext _context;
        private readonly HttpClient _httpClient;

        public BookingService(BookingDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        // ----------------------------
        // 1️⃣ Get All Bookings
        // ----------------------------
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        // ----------------------------
        // 2️⃣ Get Booking by Id
        // ----------------------------
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        }

        // ----------------------------
        // 3️⃣ Create Booking
        // ----------------------------
        public async Task<Booking> CreateBookingAsync(BookingDTO bookingDto)
        {
            // -----------------------
            // 1️⃣ Validate User via AuthAPI
            // -----------------------
            var userResponse = await _httpClient.GetAsync($"https://localhost:7001/api/auth/{bookingDto.UserId}");
            if (!userResponse.IsSuccessStatusCode)
                throw new Exception($"User with ID {bookingDto.UserId} not found in AuthAPI");

            var user = await userResponse.Content.ReadFromJsonAsync<UserDto>();
            if (user == null)
                throw new Exception($"User with ID {bookingDto.UserId} not found.");

            // -----------------------
            // 2️⃣ Validate Car via CarAPI
            // -----------------------
            var carResponse = await _httpClient.GetAsync($"https://localhost:7003/api/car/{bookingDto.CarId}");
            if (!carResponse.IsSuccessStatusCode)
                throw new Exception($"Car with ID {bookingDto.CarId} not found in CarAPI");

            var car = await carResponse.Content.ReadFromJsonAsync<CarDTO>();
            if (car == null)
                throw new Exception($"Car with ID {bookingDto.CarId} not found.");

            // -----------------------
            // 3️⃣ Validate Dates
            // -----------------------
            double hours = (bookingDto.EndDate - bookingDto.StartDate).TotalHours;
            if (hours <= 0)
                throw new Exception("EndDate must be after StartDate");

            decimal totalPrice = Math.Round((decimal)hours * car.HourlyRate, 2);

            // -----------------------
            // 4️⃣ Create Booking
            // -----------------------
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                CarId = bookingDto.CarId,
                StationId = car.StationId,
                ContractId = 0,
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
        // 5️⃣ Cancel Booking
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
    // DTO for CarAPI
    // ----------------------------
    public class CarDTO
    {
        public int Id { get; set; }
        public string Model { get; set; } = "";
        public decimal HourlyRate { get; set; }
        public int StationId { get; set; }
    }
}
