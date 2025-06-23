using System.Net.Http;
using System.Text.Json;
using MundialClubesApi.Data;
using Microsoft.EntityFrameworkCore;

public class JugadoresService
{
    private readonly HttpClient _http;
    private readonly FutbolDbContext _db;

    public JugadoresService(HttpClient http, FutbolDbContext db)
    {
        _http = http;
        _db = db;
        _http.BaseAddress = new Uri("https://v3.football.api-sports.io/");
        //API KEYS
        _http.DefaultRequestHeaders.Add("x-apisports-key", "ac0eef5a16b0719f79b4d20ba1a2cf17");
        // _http.DefaultRequestHeaders.Add("x-apisports-key", "fd83cef58d6e43dd3ff29210593e02fe");
        //FIN API KEYS
    }

    public async Task<string?> ObtenerFotoJugadorPorNombre(string nombre)
    {
        var response = await _http.GetAsync($"players?search={nombre}&season=2023");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        var jugador = doc.RootElement.GetProperty("response").EnumerateArray().FirstOrDefault();
        if (jugador.ValueKind == JsonValueKind.Undefined) return null;

        return jugador.GetProperty("player").GetProperty("photo").GetString();
    }

   public async Task<List<Jugador>> ObtenerPlantillaPorEquipo(int equipoId, int season = 2023)
    {
        // 1. Verificar si ya existen en la base de datos
        var existentes = await _db.Jugadores
            .Where(j => j.EquipoId == equipoId && j.Temporada == season)
            .ToListAsync();

        if (existentes.Any())
            return existentes;

        // 2. Si no existen, consulta la API
        var url = $"players?team={equipoId}&season={season}";
        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new List<Jugador>();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("response", out var responseArray) || responseArray.ValueKind != JsonValueKind.Array)
            return new List<Jugador>();

        var jugadores = new List<Jugador>();

        foreach (var item in responseArray.EnumerateArray())
        {
            try
            {
                var player = item.GetProperty("player");
                var statisticsArray = item.GetProperty("statistics");

                var stats = statisticsArray.EnumerateArray().FirstOrDefault();
                if (stats.ValueKind == JsonValueKind.Undefined) continue;

                var games = stats.GetProperty("games");

                var jugador = new Jugador
                {
                    Nombre = player.GetProperty("name").GetString() ?? "Desconocido",
                    Foto = player.GetProperty("photo").GetString() ?? "",
                    Numero = games.TryGetProperty("number", out var numero) && numero.ValueKind == JsonValueKind.Number ? numero.GetInt32() : 0,
                    Posicion = games.TryGetProperty("position", out var posicion) ? posicion.GetString() ?? "N/A" : "N/A",
                    EquipoId = equipoId,
                    Temporada = season
                };

                jugadores.Add(jugador);
            }
            catch
            {
                continue;
            }
        }

        // 3. Guardar en base de datos para cachear
        if (jugadores.Any())
        {
            _db.Jugadores.AddRange(jugadores);
            await _db.SaveChangesAsync();
        }

        return jugadores;
    }



}
