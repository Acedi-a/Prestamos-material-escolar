using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class RolController : ControllerBase
 {
 private readonly IRolRepositorio _roles;
 public RolController(IRolRepositorio roles) { _roles = roles; }

 // GET: api/Rol
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _roles.ListarTodosAsync();
 var result = list.Select(r => new { r.Id, r.NombreRol });
 return Ok(result);
 }

 // GET: api/Rol/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var rol = await _roles.ObtenerPorIdAsync(id);
 if (rol == null) return NotFound();
 return Ok(new { rol.Id, rol.NombreRol });
 }

 public record RolCreateRequest(string NombreRol);
 public record RolUpdateRequest(string NombreRol);

 // POST: api/Rol
 [HttpPost]
 public async Task<IActionResult> Create([FromBody] RolCreateRequest req)
 {
 if (string.IsNullOrWhiteSpace(req.NombreRol))
 return BadRequest(new { message = "El nombre del rol es requerido" });
 var entity = new Rol { NombreRol = req.NombreRol.Trim() };
 await _roles.CrearAsync(entity);
 return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new { id = entity.Id });
 }

 // PUT: api/Rol/{id}
 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] RolUpdateRequest req)
 {
 var entity = await _roles.ObtenerPorIdAsync(id);
 if (entity == null) return NotFound();
 if (string.IsNullOrWhiteSpace(req.NombreRol))
 return BadRequest(new { message = "El nombre del rol es requerido" });
 entity.NombreRol = req.NombreRol.Trim();
 await _roles.ActualizarAsync(entity);
 return Ok(new { message = "Rol actualizado" });
 }

 // DELETE: api/Rol/{id}
 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 await _roles.EliminarAsync(id);
 return Ok(new { message = "Rol eliminado" });
 }
 }
}
