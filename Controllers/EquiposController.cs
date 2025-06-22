using Microsoft.AspNetCore.Mvc;
using MundialClubesApi.Services;
using MundialClubesApi.Data;
using MundialClubesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MundialClubesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquiposController : ControllerBase
    {
        private readonly FutbolDbContext _db;

        public EquiposController(FutbolDbContext db)
        {
            _db = db;
        }

        [HttpPost("cargar/{ligaId}/{season}")]
        public async Task<IActionResult> CargarEquipos([FromRoute] int ligaId, [FromRoute] int season, [FromServices] ApiFootballService service)
        {
            await service.CargarEquiposAsync(ligaId, season);
            return Ok("Equipos cargados.");
        }

        [HttpPost("cargar-todos/{season}")]
        public async Task<IActionResult> CargarTodos([FromRoute] int season, [FromServices] ApiFootballService service)
        {
            await service.CargarTodosLosEquiposAsync(season);
            return Ok($"Equipos de todas las ligas cargados para la temporada {season}.");
        }

        [HttpGet]
        public async Task<IActionResult> GetEquipos(int page = 1)
        {
            const int pageSize = 18;
            if (page <= 0) page = 1;

            var totalEquipos = await _db.Equipos.CountAsync();
            var equipos = await _db.Equipos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                Total = totalEquipos,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalEquipos / (double)pageSize),
                Data = equipos
            });
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipo(int id)
        {
            var equipo = await _db.Equipos.FindAsync(id);
            if (equipo == null) return NotFound();
            return Ok(equipo);
        }

        [HttpGet("por-liga/{ligaId}")]
        public async Task<IActionResult> GetPorLiga(int ligaId)
        {
            var equipos = await _db.Equipos
                .Where(e => e.LigaId == ligaId)
                .ToListAsync();

            return Ok(equipos);
        }

        [HttpGet("por-pais/{pais}")]
        public async Task<IActionResult> GetPorPais(string pais)
        {
            var equipos = await _db.Equipos
                .Where(e => e.Pais != null && e.Pais.ToLower().Contains(pais.ToLower()))
                .ToListAsync();

            return Ok(equipos);
        }

        [HttpGet("{id}/partidos")]
        public async Task<IActionResult> GetPartidosEquipo(int id)
        {
            var partidos = await _db.Partidos
                .Where(p => p.EquipoLocalId == id || p.EquipoVisitanteId == id)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(partidos);
        }

        [HttpGet("{id}/jugadores")]
        public async Task<IActionResult> ObtenerJugadoresDesdeApi(int id, [FromServices] JugadoresService service)
        {
            Console.WriteLine($"🔍 Obteniendo plantilla para equipo {id}");

            var jugadores = await service.ObtenerPlantillaPorEquipo(id);
            Console.WriteLine($"👥 Total jugadores obtenidos: {jugadores.Count}");

            return Ok(jugadores);
        }




    }
}
