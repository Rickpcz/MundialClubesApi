using System.Text.Json;
using MundialClubesApi.Data;
using MundialClubesApi.Models;
using MundialClubesApi.Dtos;

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

    }
}
