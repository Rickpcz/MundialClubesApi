using Microsoft.AspNetCore.Mvc;

namespace MundialClubesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { mensaje = "API del Mundial de Clubes funcionando correctamente 🎉" });
        }
    }
}
