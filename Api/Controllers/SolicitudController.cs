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
 private readonly IDocenteRepositorio _docentes;
		private readonly AprobarSolicitud _aprobar;   
        private readonly RechazarSolicitud _rechazar;

 public SolicitudController(ISolicitudRepositorio solicitudes, ISolicitudDetalleRepositorio detalles, IMaterialRepositorio materiales, SolicitarMaterial solicitar,
	 IDocenteRepositorio docentes, AprobarSolicitud aprobar,   RechazarSolicitud rechazar)
 {
 _solicitudes = solicitudes; _detalles = detalles; _solicitar = solicitar;  _materiales = materiales; _docentes = docentes; _aprobar = aprobar;   _rechazar = rechazar; 
		}

 // GET: api/Solicitud
 [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _solicitudes.ListarTodosAsync();
            var docentes = await _docentes.ListarTodosAsync(); 

            var docenteLookup = docentes.ToDictionary(d => d.Id);

            var result = list.Select(s => {
                docenteLookup.TryGetValue(s.DocenteId, out var docente);
                return new {
                    s.Id,
                    s.DocenteId,
                    DocenteNombre = docente != null ? $"{docente.Nombre} {docente.Apellido}" : "Desconocido",
                    s.EstadoSolicitud,
                    s.FechaSolicitud
                };
            }).OrderByDescending(s => s.FechaSolicitud);
            
            return Ok(result);
        }
[HttpGet("PorDocente/{docenteId:int}")]
        public async Task<IActionResult> GetByDocenteId(int docenteId)
        {
            
            var todas = await _solicitudes.ListarTodosAsync();
            var list = todas.Where(s => s.DocenteId == docenteId)
                            .Select(s => new { s.Id, s.DocenteId, s.EstadoSolicitud, s.FechaSolicitud })
                            .OrderByDescending(s => s.FechaSolicitud); 
            return Ok(list);
        }
		// GET: api/Solicitud/{id}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var s = await _solicitudes.ObtenerPorIdAsync(id);
			if (s == null) return NotFound();

			
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
		public record AprobarRequest(DateTime FechaDevolucionPrevista);
		
		// POST: api/Solicitud/5/aprobar
		[HttpPost("{id:int}/aprobar")]
		public async Task<IActionResult> Aprobar(int id, [FromBody] AprobarRequest req) // <-- Acepta el body
		{
			try
			{
				// Validación opcional de la fecha
				if (req.FechaDevolucionPrevista <= DateTime.UtcNow)
				{
					return BadRequest(new { message = "La fecha de devolución prevista debe ser en el futuro." });
				}

				// Pasa el 'id' y la fecha del request al caso de uso
				await _aprobar.EjecutarAsync(id, req.FechaDevolucionPrevista); // <-- Pasa la fecha
				return Ok(new { message = "Solicitud Aprobada. Préstamo generado." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		public record RechazarRequest(string Motivo);

		// POST: api/Solicitud/5/rechazar
		[HttpPost("{id:int}/rechazar")]
		public async Task<IActionResult> Rechazar(int id, [FromBody] RechazarRequest? req)
		{
			try
			{
				var motivo = string.IsNullOrWhiteSpace(req?.Motivo) ? "Rechazada por el encargado" : req.Motivo;
				await _rechazar.EjecutarAsync(id, motivo);
				return Ok(new { message = "Solicitud Rechazada." });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
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
