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
 private readonly IMaterialRepositorio _materiales;
 private readonly SolicitarMaterial _solicitar;

 public SolicitudController(ISolicitudRepositorio solicitudes, ISolicitudDetalleRepositorio detalles, IMaterialRepositorio materiales, SolicitarMaterial solicitar)
 {
 _solicitudes = solicitudes; _detalles = detalles; _solicitar = solicitar;  _materiales = materiales; ;
 }

 // GET: api/Solicitud
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _solicitudes.ListarTodosAsync();
 var result = list.Select(s => new { s.Id, s.DocenteId, s.EstadoSolicitud, s.FechaSolicitud });
 return Ok(result);
 }
[HttpGet("PorDocente/{docenteId:int}")]
        public async Task<IActionResult> GetByDocenteId(int docenteId)
        {
            // (Sería ideal tener 'ListarPorDocenteIdAsync' en el repositorio,
            // pero por ahora filtramos la lista completa)
            var todas = await _solicitudes.ListarTodosAsync();
            var list = todas.Where(s => s.DocenteId == docenteId)
                            .Select(s => new { s.Id, s.DocenteId, s.EstadoSolicitud, s.FechaSolicitud })
                            .OrderByDescending(s => s.FechaSolicitud); // Ordenar por fecha
            return Ok(list);
        }
		// GET: api/Solicitud/{id}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var s = await _solicitudes.ObtenerPorIdAsync(id);
			if (s == null) return NotFound();

			// Esto es ineficiente (N+1), pero sigue tu patrón actual.
			// (Si tu 'ISolicitudDetalleRepositorio' tiene un 'ListarPorSolicitudIdAsync(id)', úsalo)
			var detallesDb = (await _detalles.ListarTodosAsync()).Where(d => d.SolicitudId == s.Id);

			var detallesConNombre = new List<object>();

			// Iteramos para buscar los nombres (N+1)
			foreach (var d in detallesDb)
			{
				var material = await _materiales.ObtenerPorIdAsync(d.MaterialId);
				detallesConNombre.Add(new
				{
					d.Id,
					d.MaterialId,
					d.CantidadSolicitada,
					NombreMaterial = material?.NombreMaterial ?? "Material no encontrado"
				});
			}

			return Ok(new { s.Id, s.DocenteId, s.EstadoSolicitud, s.FechaSolicitud, Detalles = detallesConNombre });
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
