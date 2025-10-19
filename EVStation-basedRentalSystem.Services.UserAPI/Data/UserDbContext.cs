using Microsoft.EntityFrameworkCore;
using EVStation_basedRentalSystem.Services.UserAPI.Models;

namespace EVStation_basedRentalSystem.Services.UserAPI.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-one: User ↔ Renter
            modelBuilder.Entity<Renter>()
                .HasOne(r => r.User)
                .WithOne(u => u.Renter)
                .HasForeignKey<Renter>(r => r.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one: User ↔ Staff
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.User)
                .WithOne(u => u.Staff)
                .HasForeignKey<Staff>(s => s.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one: User ↔ Admin
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
