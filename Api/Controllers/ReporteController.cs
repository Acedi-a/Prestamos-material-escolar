using Aplication.UseCases.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class ReporteController : ControllerBase
 {
 private readonly GenerarReportePrestamosYDevoluciones _generar;
 public ReporteController(GenerarReportePrestamosYDevoluciones generar) { _generar = generar; }

 // GET: api/Reporte/prestamos-y-devoluciones?desde=2025-01-01&hasta=2025-12-31&usuarioId=1
 [HttpGet("prestamos-y-devoluciones")]
 public async Task<IActionResult> Get([FromQuery] DateTime desde, [FromQuery] DateTime hasta, [FromQuery] int usuarioId)
 {
 var (prestamos, devoluciones) = await _generar.EjecutarAsync(usuarioId, desde, hasta);
 var res = new
 {
 prestamos = prestamos.Select(p => new { p.Id, p.SolicitudId, p.FechaPrestamo, p.FechaDevolucionPrevista, p.EstadoPrestamo }),
 devoluciones = devoluciones.Select(d => new { d.Id, d.PrestamoId, d.FechaDevolucion, d.Observaciones })
 };
 return Ok(res);
 }
 }
}
