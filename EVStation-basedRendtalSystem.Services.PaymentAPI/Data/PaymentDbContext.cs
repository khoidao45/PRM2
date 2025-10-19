using EVStation_basedRentalSystem.Services.PaymentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.PaymentAPI.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options) { }

        public DbSet<Payment> Payments { get; set; }
    }
}
