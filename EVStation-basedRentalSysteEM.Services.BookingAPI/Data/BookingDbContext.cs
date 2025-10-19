using EVStation_basedRentalSystem.Services.BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.BookingAPI.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: configure precision for decimal properties
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            // Optional: configure default values or relationships
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasDefaultValue("Pending");
        }
    }
}
