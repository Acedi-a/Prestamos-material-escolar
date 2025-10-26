using Aplication.UseCases.Docentes;
using Microsoft.AspNetCore.Mvc;
using Dominio.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        private readonly IDocenteRepositorio _docentes;
        private readonly RegistrarDocente _registrarDocente;

        public DocenteController(IDocenteRepositorio docentes, RegistrarDocente registrarDocente)
        {
            _docentes = docentes;
            _registrarDocente = registrarDocente;
        }

        // GET: api/Docente
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var docentes = await _docentes.ListarTodosAsync();
            // Proyección básica para evitar navegar relaciones
            var result = docentes.Select(d => new { d.Id, d.Nombre, d.Apellido, d.CedulaIdentidad, d.UsuarioId });
            return Ok(result);
        }

        // GET: api/Docente/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var docente = await _docentes.ObtenerPorIdAsync(id);
            if (docente == null) return NotFound();
            return Ok(new { docente.Id, docente.Nombre, docente.Apellido, docente.CedulaIdentidad, docente.UsuarioId });
        }

        public record RegistrarDocenteRequest(int UsuarioId, string Nombre, string Apellido, string CedulaIdentidad);

        // POST: api/Docente
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegistrarDocenteRequest request)
        {
            try
            {
                var id = await _registrarDocente.EjecutarAsync(request.Nombre, request.Apellido, request.CedulaIdentidad, request.UsuarioId);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Docente/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _docentes.EliminarAsync(id);
            return Ok(new { message = "Docente eliminado" });
        }
    }
}
