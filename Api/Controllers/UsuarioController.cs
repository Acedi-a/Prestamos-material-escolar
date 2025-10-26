using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class UsuarioController : ControllerBase
 {
 private readonly IUsuarioRepositorio _usuarios;
 private readonly IRolRepositorio _roles;
 public UsuarioController(IUsuarioRepositorio usuarios, IRolRepositorio roles)
 {
 _usuarios = usuarios; _roles = roles;
 }

 // GET: api/Usuario
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _usuarios.ListarTodosAsync();
 var result = list.Select(u => new { u.Id, u.RolId, u.NombreUsuario, u.Email });
 return Ok(result);
 }

 // GET: api/Usuario/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var u = await _usuarios.ObtenerPorIdAsync(id);
 if (u == null) return NotFound();
 return Ok(new { u.Id, u.RolId, u.NombreUsuario, u.Email });
 }

 public record UsuarioCreateRequest(int RolId, string NombreUsuario, string Email, string Contrasena);
 public record UsuarioUpdateRequest(int RolId, string NombreUsuario, string Email);

 // POST: api/Usuario
 [HttpPost]
 public async Task<IActionResult> Create([FromBody] UsuarioCreateRequest req)
 {
 if (string.IsNullOrWhiteSpace(req.NombreUsuario) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Contrasena))
 return BadRequest(new { message = "Datos requeridos: NombreUsuario, Email, Contrasena" });
 if (await _roles.ObtenerPorIdAsync(req.RolId) is null)
 return BadRequest(new { message = "Rol no existe" });
 var entity = new Usuario { RolId = req.RolId, NombreUsuario = req.NombreUsuario.Trim(), Email = req.Email.Trim(), Contrasena = req.Contrasena };
 await _usuarios.CrearAsync(entity);
 return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new { id = entity.Id });
 }

 // PUT: api/Usuario/{id}
 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateRequest req)
 {
 var entity = await _usuarios.ObtenerPorIdAsync(id);
 if (entity == null) return NotFound();
 if (await _roles.ObtenerPorIdAsync(req.RolId) is null)
 return BadRequest(new { message = "Rol no existe" });
 if (string.IsNullOrWhiteSpace(req.NombreUsuario) || string.IsNullOrWhiteSpace(req.Email))
 return BadRequest(new { message = "Datos requeridos: NombreUsuario, Email" });
 entity.RolId = req.RolId;
 entity.NombreUsuario = req.NombreUsuario.Trim();
 entity.Email = req.Email.Trim();
 await _usuarios.ActualizarAsync(entity);
 return Ok(new { message = "Usuario actualizado" });
 }

 // DELETE: api/Usuario/{id}
 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 await _usuarios.EliminarAsync(id);
 return Ok(new { message = "Usuario eliminado" });
 }
 }
}
