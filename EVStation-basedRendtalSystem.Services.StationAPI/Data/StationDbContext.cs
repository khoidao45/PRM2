using System.Collections.Generic;
using EVStation_basedRentalSystem.Services.StationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EVStation_basedRentalSystem.Services.StationAPI.Data
{
    public class StationDbContext : DbContext
    {
        public StationDbContext(DbContextOptions<StationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Stations { get; set; }
    }
}