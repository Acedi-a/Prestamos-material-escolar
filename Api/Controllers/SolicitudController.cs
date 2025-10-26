using Aplication.UseCases.Solicitudes;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class SolicitudController : ControllerBase
 {
 private readonly ISolicitudRepositorio _solicitudes;
 private readonly ISolicitudDetalleRepositorio _detalles;
 private readonly SolicitarMaterial _solicitar;

 public SolicitudController(ISolicitudRepositorio solicitudes, ISolicitudDetalleRepositorio detalles, SolicitarMaterial solicitar)
 {
 _solicitudes = solicitudes; _detalles = detalles; _solicitar = solicitar;
 }

 // GET: api/Solicitud
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _solicitudes.ListarTodosAsync();
 var result = list.Select(s => new { s.Id, s.DocenteId, s.EstadoSolicitud, s.FechaSolicitud });
 return Ok(result);
 }

 // GET: api/Solicitud/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var s = await _solicitudes.ObtenerPorIdAsync(id);
 if (s == null) return NotFound();
 var detalles = (await _detalles.ListarTodosAsync()).Where(d => d.SolicitudId == s.Id).Select(d => new { d.Id, d.MaterialId, d.CantidadSolicitada });
 return Ok(new { s.Id, s.DocenteId, s.EstadoSolicitud, s.FechaSolicitud, Detalles = detalles });
 }

 public record SolicitarItem(int MaterialId, int Cantidad);
 public record SolicitarReq(int DocenteId, IEnumerable<SolicitarItem> Items);

 // POST: api/Solicitud
 [HttpPost]
 public async Task<IActionResult> Crear([FromBody] SolicitarReq req)
 {
 try
 {
 var id = await _solicitar.EjecutarAsync(req.DocenteId, req.Items.Select(i => (i.MaterialId, i.Cantidad)));
 return CreatedAtAction(nameof(GetById), new { id }, new { id });
 }
 catch (ArgumentException ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }
 }
}
