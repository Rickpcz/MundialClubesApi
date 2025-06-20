using System.Net.Http;
using System.Text.Json;
using MundialClubesApi.Dtos;

public class TransferenciasService
{
    private readonly HttpClient _http;

    public TransferenciasService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FichajeDto>> ObtenerFichajesAsync()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://live-football-transfers-in-europe.p.rapidapi.com/news"),
        };

        request.Headers.Add("X-RapidAPI-Key", "efc1a802dbmsh8d087da45ad37b3p1dbfbajsnefad70bc11c5");
        request.Headers.Add("X-RapidAPI-Host", "live-football-transfers-in-europe.p.rapidapi.com");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<List<FichajeDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data ?? new();
    }
}
