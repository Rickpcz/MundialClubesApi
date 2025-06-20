using System.Net.Http.Json;
using MundialClubesApi.Dtos;

namespace MundialClubesApi.Services
{
    public class TheSportsDbService
    {
        private readonly HttpClient _http;
        private readonly Dictionary<string, string> _cacheLigaLogos = new();

        public TheSportsDbService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://www.thesportsdb.com/api/v1/json/123/");
        }

        public async Task<List<PartidoExternoDto>> ObtenerPartidosDelDia(string fecha)
        {
            var url = $"eventsday.php?d={fecha}&s=Soccer";
            var response = await _http.GetFromJsonAsync<TheSportsDbResponse>(url);

            if (response?.Events == null) return new();

            var partidos = await Task.WhenAll(response.Events.Select(async e => new PartidoExternoDto
            {
                IdEvento = e.IdEvent ?? string.Empty,
                Local = e.StrHomeTeam ?? string.Empty,
                Visitante = e.StrAwayTeam ?? string.Empty,
                LogoLocal = e.StrHomeTeamBadge ?? string.Empty,
                LogoVisitante = e.StrAwayTeamBadge ?? string.Empty,
                GolesLocal = e.IntHomeScore,
                GolesVisitante = e.IntAwayScore,
                Hora = e.StrTime ?? string.Empty,
                Estado = e.StrStatus ?? string.Empty,
                Liga = e.StrLeague ?? string.Empty,
                LigaLogo = await ObtenerLogoLigaAsync(e.IdLeague ?? string.Empty)
            }));

            return partidos.ToList();
        }

        private async Task<string> ObtenerLogoLigaAsync(string idLeague)
        {
            if (string.IsNullOrWhiteSpace(idLeague))
                return string.Empty;

            if (_cacheLigaLogos.TryGetValue(idLeague, out var logo))
                return logo;

            var url = $"lookupleague.php?id={idLeague}";
            var res = await _http.GetFromJsonAsync<LeagueLookupResponse>(url);

            var badge = res?.Leagues?.FirstOrDefault()?.StrBadge ?? string.Empty;

            _cacheLigaLogos[idLeague] = badge;
            return badge;
        }

        private class TheSportsDbResponse
        {
            public List<Evento>? Events { get; set; }
        }

        private class Evento
        {
            public string? IdEvent { get; set; }
            public string? StrHomeTeam { get; set; }
            public string? StrAwayTeam { get; set; }
            public string? StrHomeTeamBadge { get; set; }
            public string? StrAwayTeamBadge { get; set; }
            public int? IntHomeScore { get; set; }
            public int? IntAwayScore { get; set; }
            public string? StrTime { get; set; }
            public string? StrStatus { get; set; }
            public string? StrLeague { get; set; }
            public string? IdLeague { get; set; }  // IMPORTANTE para obtener el logo
        }

        private class LeagueLookupResponse
        {
            public List<LeagueInfo>? Leagues { get; set; }
        }

        private class LeagueInfo
        {
            public string? StrBadge { get; set; }
        }
    }
}
