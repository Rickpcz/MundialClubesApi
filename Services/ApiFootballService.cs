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



            if (result?.Response == null || result.Response.Count == 0)
            {
                Console.WriteLine("❌ No se recibieron equipos desde la API.");
                return;
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
                        LigaId = ligaId
                    });
                    nuevos++;
                }
            }

            await _db.SaveChangesAsync();

        }

        public async Task CargarTodosLosEquiposAsync(int season)
        {
            var ligas = await _db.Ligas.ToListAsync();
            int totalLigas = ligas.Count;
            int totalEquipos = 0;



            foreach (var liga in ligas)
            {


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

            }


        }

        public async Task CargarPartidosAsync(int ligaId, int season)
        {
            var url = $"fixtures?league={ligaId}&season={season}";
            var response = await _http.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiFootballResponse<FixtureWrapper>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result?.Response == null || result.Response.Count == 0)
            {
                Console.WriteLine("❌ No se recibieron partidos desde la API.");
                return;
            }



            int nuevos = 0;

            foreach (var f in result.Response)
            {
                if (!_db.Partidos.Any(p => p.Id == f.Fixture.Id))
                {
                    _db.Partidos.Add(new Partido
                    {
                        Id = f.Fixture.Id,
                        LigaId = ligaId,
                        Fecha = DateTime.Parse(f.Fixture.Date),
                        EquipoLocalId = f.Teams.Home.Id,
                        EquipoVisitanteId = f.Teams.Away.Id,
                        GolesLocal = f.Goals.Home,
                        GolesVisitante = f.Goals.Away,
                        Estado = f.Fixture.Status,
                    });
                    nuevos++;
                }
            }

            await _db.SaveChangesAsync();

        }

        public async Task<string> ObtenerDetallePartidoAsync(int fixtureId)
        {
            var alineaciones = await _db.Alineaciones
                .Include(a => a.Jugadores)
                    .ThenInclude(ja => ja.Jugador)
                .Where(a => a.PartidoId == fixtureId)
                .ToListAsync();

            var estadisticas = await _db.EstadisticasEquipo
                .Where(e => e.PartidoId == fixtureId)
                .ToListAsync();

            var eventos = await _db.EventosPartido
                .Where(ev => ev.PartidoId == fixtureId)
                .ToListAsync();

            if (alineaciones.Any() || estadisticas.Any() || eventos.Any())
            {
                var resultadoExistente = new
                {
                    lineups = alineaciones,
                    statistics = estadisticas,
                    events = eventos
                };
                return JsonSerializer.Serialize(resultadoExistente, new JsonSerializerOptions { WriteIndented = true });
            }

            // Llamadas a la API
            var urlLineups = $"fixtures/lineups?fixture={fixtureId}";
            var urlStats = $"fixtures/statistics?fixture={fixtureId}";
            var urlEvents = $"fixtures/events?fixture={fixtureId}";

            var lineupsJson = await _http.GetStringAsync(urlLineups);
            var statsJson = await _http.GetStringAsync(urlStats);
            var eventsJson = await _http.GetStringAsync(urlEvents);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var lineups = JsonSerializer.Deserialize<ApiFootballResponse<LineupDto>>(lineupsJson, options);
            var stats = JsonSerializer.Deserialize<ApiFootballResponse<StatisticDto>>(statsJson, options);
            var events = JsonSerializer.Deserialize<ApiFootballResponse<EventoDto>>(eventsJson, options);

            // Procesar y guardar alineaciones
            if (lineups?.Response != null)
            {
                foreach (LineupDto team in lineups.Response)
                {
                    var alineacion = new Alineacion
                    {
                        PartidoId = fixtureId,
                        EquipoId = team.Team.Id,
                        Formacion = team.Formation,
                        Jugadores = new List<JugadorAlineacion>()
                    };

                    foreach (PlayerWrapper j in team.StartXI)
                    {
                        var jugadorExistente = _db.Jugadores.FirstOrDefault(x => x.Nombre == j.Player.Name);
                        Jugador jugador = jugadorExistente ?? new Jugador
                        {
                            Nombre = j.Player.Name,
                            Numero = j.Player.Number,
                            Posicion = j.Player.Pos,
                            Foto = j.Player.Photo
                        };

                        if (jugadorExistente == null) _db.Jugadores.Add(jugador);

                        alineacion.Jugadores.Add(new JugadorAlineacion
                        {
                            Posicion = j.Player.Pos,
                            Jugador = jugador
                        });
                    }

                    _db.Alineaciones.Add(alineacion);
                }
            }

            // Procesar y guardar estadísticas
            if (stats?.Response != null)
            {
                foreach (var teamStat in stats.Response)
                {
                    foreach (StatEntry s in teamStat.Statistics)
                    {
                        _db.EstadisticasEquipo.Add(new EstadisticaEquipo
                        {
                            PartidoId = fixtureId,
                            EquipoId = teamStat.Team.Id,
                            Tipo = s.Type,
                            Valor = s.Value?.ToString() ?? "0"
                        });
                    }
                }
            }

            // Procesar y guardar eventos
            if (events?.Response is List<EventoDto> eventosList)
            {
                foreach (EventoDto e in eventosList)
                {
                    _db.EventosPartido.Add(new EventoPartido
                    {
                        PartidoId = fixtureId,
                        Tiempo = $"{e.Time.Elapsed}+{(e.Time.Extra ?? 0)}",
                        Tipo = e.Type,
                        Jugador = e.Player?.Name ?? "Sin nombre",
                        Detalle = e.Detail,
                        EquipoId = e.Team.Id
                    });
                }
            }

            await _db.SaveChangesAsync();

            var resultado = new
            {
                lineups = lineups?.Response,
                statistics = stats?.Response,
                events = events?.Response
            };

            return JsonSerializer.Serialize(resultado, new JsonSerializerOptions { WriteIndented = true });
        }

         public async Task<List<StandingDto>> ObtenerTablaPorLigaTemporada(int leagueId, int season)
    {
        var response = await _http.GetAsync($"standings?league={leagueId}&season={season}");
        if (!response.IsSuccessStatusCode) return new List<StandingDto>();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var tabla = new List<StandingDto>();

        var standingsArray = doc.RootElement
            .GetProperty("response")[0]
            .GetProperty("league")
            .GetProperty("standings")[0];

        foreach (var equipo in standingsArray.EnumerateArray())
        {
            tabla.Add(new StandingDto
            {
                Posicion = equipo.GetProperty("rank").GetInt32(),
                NombreEquipo = equipo.GetProperty("team").GetProperty("name").GetString() ?? "",
                Logo = equipo.GetProperty("team").GetProperty("logo").GetString() ?? "",
                Puntos = equipo.GetProperty("points").GetInt32(),
                PartidosJugados = equipo.GetProperty("all").GetProperty("played").GetInt32(),
                Ganados = equipo.GetProperty("all").GetProperty("win").GetInt32(),
                Empatados = equipo.GetProperty("all").GetProperty("draw").GetInt32(),
                Perdidos = equipo.GetProperty("all").GetProperty("lose").GetInt32(),
                GolesFavor = equipo.GetProperty("all").GetProperty("goals").GetProperty("for").GetInt32(),
                GolesContra = equipo.GetProperty("all").GetProperty("goals").GetProperty("against").GetInt32(),
                Grupo = equipo.TryGetProperty("group", out var g) ? g.GetString() ?? "" : ""
            });
        }

        return tabla;
    }

    }
}
