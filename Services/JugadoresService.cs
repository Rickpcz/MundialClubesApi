using System.Net.Http;
using System.Text.Json;

public class JugadoresService
{
    private readonly HttpClient _http;

    public JugadoresService(HttpClient http)
    {
        _http = http;
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

                // Prioriza Premier League u otra si quieres
                var stats = statisticsArray.EnumerateArray().FirstOrDefault();
                var games = stats.GetProperty("games");

                jugadores.Add(new Jugador
                {
                    Nombre = player.GetProperty("name").GetString() ?? "Desconocido",
                    Foto = player.GetProperty("photo").GetString() ?? "",
                    Numero = games.TryGetProperty("number", out var numero) && numero.ValueKind == JsonValueKind.Number ? numero.GetInt32() : 0,
                    Posicion = games.TryGetProperty("position", out var posicion) ? posicion.GetString() ?? "N/A" : "N/A"
                });
            }
            catch
            {
                continue;
            }
        }

        return jugadores;
    }



}
