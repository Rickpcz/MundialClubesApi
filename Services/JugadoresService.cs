using System.Net.Http;
using System.Text.Json;

public class JugadoresService
{
    private readonly HttpClient _http;

    public JugadoresService(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://v3.football.api-sports.io/");
        _http.DefaultRequestHeaders.Add("x-apisports-key", "ac0eef5a16b0719f79b4d20ba1a2cf17");
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
}
