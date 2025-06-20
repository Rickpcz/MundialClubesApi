// Controllers/TransferenciasController.cs
using Microsoft.AspNetCore.Mvc;
using MundialClubesApi.Dtos;
using MundialClubesApi.Services;

[ApiController]
[Route("api/[controller]")]
public class TransferenciasController : ControllerBase
{
    private readonly TransferenciasService _service;
    private readonly JugadoresService _jugadoresService;


    public TransferenciasController(TransferenciasService service, JugadoresService jugadoresService)
    {
        _service = service;
        _jugadoresService = jugadoresService;
    }


    [HttpGet("recientes")]
    public async Task<ActionResult<List<FichajeDto>>> ObtenerRecientes()
    {
        var datos = await _service.ObtenerFichajesAsync();
        return Ok(datos);
    }
    [HttpGet("foto-jugador/{nombre}")]
    public async Task<IActionResult> FotoJugador(string nombre)
    {
        var foto = await _jugadoresService.ObtenerFotoJugadorPorNombre(nombre);
        return Ok(new { nombre, foto });
    }
}
