using Microsoft.EntityFrameworkCore;
using MundialClubesApi.Models;

namespace MundialClubesApi.Data
{
    public class FutbolDbContext : DbContext
    {
        public FutbolDbContext(DbContextOptions<FutbolDbContext> options) : base(options) { }

        public DbSet<Liga> Ligas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Partido> Partidos { get; set; }

        public DbSet<Alineacion> Alineaciones { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<JugadorAlineacion> JugadoresAlineacion { get; set; }
        public DbSet<EstadisticaEquipo> EstadisticasEquipo { get; set; }
        public DbSet<EventoPartido> EventosPartido { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<ResumenTemporada> ResumenTemporada { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Partido>().OwnsOne(p => p.Estado);
            modelBuilder.Entity<JugadorAlineacion>()
                .HasOne(j => j.Jugador)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
