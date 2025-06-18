using Microsoft.AspNetCore.Mvc;
using MundialClubesApi.Services;

namespace MundialClubesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquiposController : ControllerBase
    {
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

    }


}
