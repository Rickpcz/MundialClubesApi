using Microsoft.AspNetCore.Mvc;
using MundialClubesApi.Data;
using MundialClubesApi.Services;
using Microsoft.EntityFrameworkCore;
using MundialClubesApi.Models;

namespace MundialClubesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LigasController : ControllerBase
    {
        private readonly FutbolDbContext _db;

        public LigasController(FutbolDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetLigas()
        {
            var ligas = await _db.Ligas.ToListAsync();
            return Ok(ligas);
        }

        [HttpPost("cargar")]
        public async Task<IActionResult> CargarLigas([FromServices] ApiFootballService service)
        {
            await service.CargarLigasAsync();
            return Ok("Ligas cargadas.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Liga>> GetLiga(int id)
        {
            var liga = await _db.Ligas.FindAsync(id);
            if (liga == null) return NotFound();
            return Ok(liga);
        }

        [HttpGet("pais/{pais}")]
        public async Task<ActionResult<IEnumerable<Liga>>> GetLigasPorPais(string pais)
        {
            var ligas = await _db.Ligas
                .Where(l => l.Pais != null && l.Pais.ToLower().Contains(pais.ToLower()))
                .ToListAsync();
            return Ok(ligas);
        }

        [HttpGet("{id}/equipos")]
        public async Task<IActionResult> GetEquiposDeLiga(int id)
        {
            var equipos = await _db.Equipos
                .Where(e => e.LigaId == id)
                .ToListAsync();

            if (!equipos.Any()) return NotFound("No se encontraron equipos para esta liga.");
            return Ok(equipos);
        }

        [HttpGet("{id}/partidos/{season}")]
        public async Task<IActionResult> GetPartidosDeLiga(int id, int season)
        {
            var partidos = await _db.Partidos
                .Where(p => p.LigaId == id && p.Fecha.Year == season)
                .ToListAsync();

            if (!partidos.Any()) return NotFound("No se encontraron partidos para esta liga y temporada.");
            return Ok(partidos);
        }
    }
}
