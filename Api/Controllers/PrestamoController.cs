using Aplication.UseCases.Prestamos;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class PrestamoController : ControllerBase
 {
 private readonly IPrestamoRepositorio _prestamos;
 private readonly IPrestamoDetalleRepositorio _detalles;
 private readonly RegistrarPrestamo _registrar;

 public PrestamoController(IPrestamoRepositorio prestamos, IPrestamoDetalleRepositorio detalles, RegistrarPrestamo registrar)
 {
 _prestamos = prestamos; _detalles = detalles; _registrar = registrar;
 }

 // GET: api/Prestamo
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _prestamos.ListarTodosAsync();
 var result = list.Select(p => new { p.Id, p.SolicitudId, p.FechaPrestamo, p.FechaDevolucionPrevista, p.EstadoPrestamo });
 return Ok(result);
 }

 // GET: api/Prestamo/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var p = await _prestamos.ObtenerPorIdAsync(id);
 if (p == null) return NotFound();
 var detalles = (await _detalles.ListarTodosAsync()).Where(d => d.PrestamoId == p.Id).Select(d => new { d.Id, d.MaterialId, d.CantidadPrestada });
 return Ok(new { p.Id, p.SolicitudId, p.FechaPrestamo, p.FechaDevolucionPrevista, p.EstadoPrestamo, Detalles = detalles });
 }

 public record RegistrarPrestamoRequest(int SolicitudId, DateTime FechaDevolucionPrevista);

 // POST: api/Prestamo
 [HttpPost]
 public async Task<IActionResult> Registrar([FromBody] RegistrarPrestamoRequest req)
 {
 try
 {
 var id = await _registrar.EjecutarAsync(req.SolicitudId, req.FechaDevolucionPrevista);
 return CreatedAtAction(nameof(GetById), new { id }, new { id });
 }
 catch (Exception ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }
 }
}
