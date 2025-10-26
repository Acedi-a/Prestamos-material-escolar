using Aplication.UseCases.Movimientos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly ConsultarHistorialMovimientos _consultar;

        public MovimientoController(ConsultarHistorialMovimientos consultar)
        {
            _consultar = consultar;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? materialId)
        {
            var lista = await _consultar.EjecutarAsync(materialId);
            return Ok(lista);
        }
    }
}
