using Microsoft.EntityFrameworkCore;
using MundialClubesApi.Models;

namespace MundialClubesApi.Data
{
    public class FutbolDbContext : DbContext
    {
        public FutbolDbContext(DbContextOptions<FutbolDbContext> options) : base(options) { }

        public DbSet<Liga> Ligas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }

    }
}
