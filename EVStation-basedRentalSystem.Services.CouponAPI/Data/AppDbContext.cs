using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RenterProfile> RenterProfiles { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<AdminProfile> AdminProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configure 1:1 using same key
            builder.Entity<RenterProfile>()
                   .HasKey(r => r.Id);

            builder.Entity<RenterProfile>()
                   .HasOne(r => r.User)
                   .WithOne(u => u.RenterProfile)
                   .HasForeignKey<RenterProfile>(r => r.Id);

            builder.Entity<StaffProfile>()
                   .HasKey(s => s.Id);

            builder.Entity<StaffProfile>()
                   .HasOne(s => s.User)
                   .WithOne(u => u.StaffProfile)
                   .HasForeignKey<StaffProfile>(s => s.Id);

            builder.Entity<AdminProfile>()
                   .HasKey(a => a.Id);

            builder.Entity<AdminProfile>()
                   .HasOne(a => a.User)
                   .WithOne(u => u.AdminProfile)
                   .HasForeignKey<AdminProfile>(a => a.Id);
        }
    }
}
