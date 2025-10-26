using Aplication.UseCases.Reparaciones;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class ReparacionController : ControllerBase
 {
 private readonly RegistrarEnvioAReparacion _registrar;
 private readonly IHistorialReparacionRepositorio _historial;
 private readonly ConsultarHistorialReparaciones _consultar;

 public ReparacionController(RegistrarEnvioAReparacion registrar, IHistorialReparacionRepositorio historial, ConsultarHistorialReparaciones consultar)
 {
 _registrar = registrar; _historial = historial; _consultar = consultar;
 }

 public record EnviarReq(int MaterialId, DateTime FechaEnvio, string DescripcionFalla, decimal? Costo, int Cantidad);

 // POST: api/Reparacion
 [HttpPost]
 public async Task<IActionResult> Enviar([FromBody] EnviarReq req)
 {
 try
 {
 var id = await _registrar.EjecutarAsync(req.MaterialId, req.FechaEnvio, req.DescripcionFalla, req.Costo, req.Cantidad);
 return CreatedAtAction(nameof(GetById), new { id }, new { id });
 }
 catch (Exception ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }

 // GET: api/Reparacion
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var reparaciones = await _consultar.EjecutarAsync();

 return Ok(reparaciones);
 }

 // GET: api/Reparacion/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var h = await _historial.ObtenerPorIdAsync(id);
 if (h == null) return NotFound();
 return Ok(new { h.Id, h.MaterialId, h.FechaEnvio, h.FechaRetorno, h.DescripcionFalla, h.Costo, h.Cantidad });
 }
 }
}
