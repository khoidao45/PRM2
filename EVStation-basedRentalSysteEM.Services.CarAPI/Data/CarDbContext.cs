using Microsoft.EntityFrameworkCore;
using EVStation_basedRentalSystem.Services.CarAPI.Models;

namespace EVStation_basedRentalSystem.Services.CarAPI.Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(c => c.Id).ValueGeneratedOnAdd(); // important!
                entity.Property(c => c.BatteryCapacity).HasPrecision(10, 2);
                entity.Property(c => c.CurrentBatteryLevel).HasPrecision(10, 2);
                entity.Property(c => c.HourlyRate).HasPrecision(10, 2);
                entity.Property(c => c.DailyRate).HasPrecision(10, 2);
            });
        }
    }
}
