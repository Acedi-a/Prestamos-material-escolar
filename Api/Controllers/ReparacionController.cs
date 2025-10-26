using Aplication.UseCases.Reparaciones;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class ReparacionController : ControllerBase
 {
 private readonly RegistrarEnvioAReparacion _registrar;
 public ReparacionController(RegistrarEnvioAReparacion registrar) { _registrar = registrar; }

 public record EnviarReq(int MaterialId, DateTime FechaEnvio, string DescripcionFalla, decimal? Costo);

 // POST: api/Reparacion
 [HttpPost]
 public async Task<IActionResult> Enviar([FromBody] EnviarReq req)
 {
 try
 {
 var id = await _registrar.EjecutarAsync(req.MaterialId, req.FechaEnvio, req.DescripcionFalla, req.Costo);
 return CreatedAtAction(nameof(Enviar), new { id }, new { id });
 }
 catch (Exception ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }
 }
}
