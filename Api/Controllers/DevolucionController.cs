using Aplication.UseCases.Devoluciones;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class DevolucionController : ControllerBase
 {
 private readonly IDevolucionRepositorio _devoluciones;
 private readonly RegistrarDevolucion _registrar;

 public DevolucionController(IDevolucionRepositorio devoluciones, RegistrarDevolucion registrar)
 {
 _devoluciones = devoluciones; _registrar = registrar;
 }

 // GET: api/Devolucion
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _devoluciones.ListarTodosAsync();
 var result = list.Select(d => new { d.Id, d.PrestamoId, d.FechaDevolucion, d.Observaciones });
 return Ok(result);
 }

 public record RegistrarDevolucionRequest(int PrestamoId, string? Observaciones);

 // POST: api/Devolucion
 [HttpPost]
 public async Task<IActionResult> Registrar([FromBody] RegistrarDevolucionRequest req)
 {
 try
 {
 var id = await _registrar.EjecutarAsync(req.PrestamoId, req.Observaciones);
 return CreatedAtAction(nameof(GetAll), new { id }, new { id });
 }
 catch (Exception ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }
 }
}
