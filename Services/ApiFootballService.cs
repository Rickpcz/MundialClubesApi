using System.Text.Json;
using MundialClubesApi.Data;
using MundialClubesApi.Models;
using MundialClubesApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MundialClubesApi.Services
{
    public class ApiFootballService
    {
        private readonly HttpClient _http;
        private readonly FutbolDbContext _db;

        public ApiFootballService(HttpClient http, FutbolDbContext db)
        {
            _http = http;
            _db = db;
        }

        public async Task CargarLigasAsync()
        {
            var response = await _http.GetAsync("leagues");
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiFootballResponse<LeagueWrapper>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            if (result?.Response == null || result.Response.Count == 0)
            {
                Console.WriteLine("❌ No se recibieron ligas desde la API.");
                return;
            }


            if (result.Response.Count == 0)
            {
                Console.WriteLine("⚠️ No se recibieron ligas desde la API.");
                return;
            }



            int nuevas = 0;

            foreach (var l in result.Response)
            {
                var id = l.League.Id;
                var nombre = l.League.Name;

                if (!_db.Ligas.Any(x => x.Id == id))
                {
                    _db.Ligas.Add(new Liga
                    {
                        Id = id,
                        Nombre = nombre,
                        Tipo = l.League.Type,
                        Logo = l.League.Logo,
                        Pais = l.Country?.Name ?? "Desconocido"
                    });
                    nuevas++;
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task CargarEquiposAsync(int ligaId, int season)
        {
            var url = $"teams?league={ligaId}&season={season}";
            var response = await _http.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiFootballResponse<TeamWrapper>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Console.WriteLine(result);


            if (result?.Response == null || result.Response.Count == 0)
            {
                Console.WriteLine("❌ No se recibieron equipos desde la API.");
                return;
            }

            Console.WriteLine($"✅ Se recibieron {result.Response.Count} equipos.");

            int nuevos = 0;

            foreach (var e in result.Response)
            {
                if (!_db.Equipos.Any(x => x.Id == e.Team.Id))
                {
                    _db.Equipos.Add(new Equipo
                    {
                        Id = e.Team.Id,
                        Nombre = e.Team.Name,
                        Logo = e.Team.Logo,
                        Pais = e.Country?.Name ?? "Desconocido",
                        LigaId = ligaId
                    });
                    nuevos++;
                }
            }

            await _db.SaveChangesAsync();
            Console.WriteLine($"✅ {nuevos} equipos nuevos guardados.");
        }

        public async Task CargarTodosLosEquiposAsync(int season)
        {
            var ligas = await _db.Ligas.ToListAsync();
            int totalLigas = ligas.Count;
            int totalEquipos = 0;

            Console.WriteLine($"🔄 Cargando equipos de {totalLigas} ligas para la temporada {season}...");

            foreach (var liga in ligas)
            {
                Console.WriteLine($"➡️ Liga: {liga.Nombre} ({liga.Id})");

                var url = $"teams?league={liga.Id}&season={season}";
                var response = await _http.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiFootballResponse<TeamWrapper>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Response == null || result.Response.Count == 0)
                {
                    Console.WriteLine($"⚠️ No se recibieron equipos para la liga {liga.Nombre}");
                    continue;
                }

                int nuevos = 0;

                foreach (var e in result.Response)
                {
                    if (!_db.Equipos.Any(x => x.Id == e.Team.Id))
                    {
                        _db.Equipos.Add(new Equipo
                        {
                            Id = e.Team.Id,
                            Nombre = e.Team.Name,
                            Logo = e.Team.Logo,
                            Pais = e.Country?.Name ?? "Desconocido",
                            LigaId = liga.Id
                        });
                        nuevos++;
                        totalEquipos++;
                    }
                }

                await _db.SaveChangesAsync();
                Console.WriteLine($"✅ {nuevos} equipos guardados para {liga.Nombre}");
            }

            Console.WriteLine($"🎉 Total de equipos nuevos guardados: {totalEquipos}");
        }


    }
}
