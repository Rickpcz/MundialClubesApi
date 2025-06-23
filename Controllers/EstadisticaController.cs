using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MundialClubesApi.Services;
using MundialClubesApi.Dtos;

namespace MundialClubesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadisticaController : ControllerBase
    {
        private readonly ApiFootballService _apiFootballService;

        public EstadisticaController(ApiFootballService apiFootballService)
        {
            _apiFootballService = apiFootballService;
        }

        [HttpGet("resumen-temporada")]
        public async Task<ActionResult<ResumenTemporadaDto>> ObtenerResumenTemporada([FromQuery] int ligaId, [FromQuery] int temporada)
        {
            if (ligaId <= 0 || temporada <= 0)
                return BadRequest("Parámetros inválidos.");

            var resumen = await _apiFootballService.ObtenerYGuardarResumenTemporada(ligaId, temporada);
            return Ok(resumen);
        }
    }
}
