using Microsoft.AspNetCore.Mvc;
using MundialClubesApi.Data;
using MundialClubesApi.Models;
using MundialClubesApi.Services;
using Microsoft.EntityFrameworkCore;

namespace MundialClubesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly FutbolDbContext _db;

        public PartidosController(FutbolDbContext db)
        {
            _db = db;
        }

        [HttpPost("partidos/cargar/{ligaId}/{season}")]
        public async Task<IActionResult> CargarPartidos([FromRoute] int ligaId, [FromRoute] int season, [FromServices] ApiFootballService service)
        {
            await service.CargarPartidosAsync(ligaId, season);
            return Ok("Partidos cargados.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var partidos = await _db.Partidos
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(partidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var partido = await _db.Partidos.FindAsync(id);
            if (partido == null) return NotFound();
            return Ok(partido);
        }

        [HttpGet("por-liga/{ligaId}/{season}")]
        public async Task<IActionResult> GetPorLiga(int ligaId, int season)
        {
            var partidos = await _db.Partidos
                .Where(p => p.LigaId == ligaId && p.Fecha.Year == season)
                .OrderBy(p => p.Fecha)
                .ToListAsync();

            return Ok(partidos);
        }

        [HttpGet("por-equipo/{equipoId}")]
        public async Task<IActionResult> GetPorEquipo(int equipoId)
        {
            var partidos = await _db.Partidos
                .Where(p => p.EquipoLocalId == equipoId || p.EquipoVisitanteId == equipoId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(partidos);
        }
        [HttpGet("detalle/{fixtureId}")]
        public async Task<IActionResult> ObtenerDetalle(int fixtureId, [FromServices] ApiFootballService service)
        {
            var resultadoJson = await service.ObtenerDetallePartidoAsync(fixtureId);
            return Content(resultadoJson, "application/json");
        }

        [HttpGet("por-liga/{ligaId}")]
        public async Task<IActionResult> ObtenerPorLiga(int ligaId)
        {
            var partidos = await _db.Partidos.Where(p => p.LigaId == ligaId).ToListAsync();
            return Ok(partidos);
        }

        [HttpGet("por-fecha/{fecha}")]
        public async Task<IActionResult> ObtenerPorFecha(DateTime fecha)
        {
            var partidos = await _db.Partidos
                .Where(p => p.Fecha.Date == fecha.Date)
                .ToListAsync();
            return Ok(partidos);
        }

        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> ObtenerPorEstado(string estado)
        {
            var partidos = await _db.Partidos
                .Where(p => p.Estado.Long.ToLower().Contains(estado.ToLower()))
                .ToListAsync();
            return Ok(partidos);
        }



    }
}
