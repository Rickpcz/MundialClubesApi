using Microsoft.AspNetCore.Mvc;
using MundialClubesApi.Data;
using MundialClubesApi.Services;
using Microsoft.EntityFrameworkCore;

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
    }
}
