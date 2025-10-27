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
 private readonly CompletarReparacion _completar;
 private readonly IHistorialReparacionRepositorio _historial;
 private readonly ConsultarHistorialReparaciones _consultar;

 public ReparacionController(RegistrarEnvioAReparacion registrar, CompletarReparacion completar, IHistorialReparacionRepositorio historial, ConsultarHistorialReparaciones consultar)
 {
 _registrar = registrar; _completar = completar; _historial = historial; _consultar = consultar;
 }

 public record EnviarReq(int MaterialId, DateTime FechaEnvio, string DescripcionFalla, decimal? Costo, int Cantidad);
 public record CompletarReq(int ReparacionId, DateTime? FechaRetorno);

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

 // POST: api/Reparacion/completar
 [HttpPost("completar")]
 public async Task<IActionResult> Completar([FromBody] CompletarReq req)
 {
 try
 {
 await _completar.EjecutarAsync(req.ReparacionId, req.FechaRetorno);
 return Ok(new { message = "Reparación completada" });
 }
 catch (Exception ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }

 // GET: api/Reparacion
 [HttpGet]
 public async Task<IActionResult> GetAll([FromQuery] int? materialId)
 {
 var list = await _consultar.EjecutarAsync(materialId);
 var result = list.Select(h => new { h.Id, h.MaterialId, h.FechaEnvio, h.FechaRetorno, h.DescripcionFalla, h.Costo, h.Cantidad });
 return Ok(result);
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
