using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class AuthController : ControllerBase
 {
 private readonly IUsuarioRepositorio _usuarios;
 private readonly IJwtTokenService _jwt;

 public AuthController(IUsuarioRepositorio usuarios, IJwtTokenService jwt)
 {
 _usuarios = usuarios; _jwt = jwt;
 }

 public record LoginRequest(string NombreUsuarioOrEmail, string Password);
 public record LoginResponse(int UsuarioId, int RolId, string NombreUsuario, string Email, string Token);

 // POST: api/Auth/login
 [HttpPost("login")]
 public async Task<IActionResult> Login([FromBody] LoginRequest req)
 {
 if (string.IsNullOrWhiteSpace(req.NombreUsuarioOrEmail) || string.IsNullOrWhiteSpace(req.Password))
 return BadRequest(new { message = "Credenciales requeridas" });

 var users = await _usuarios.ListarTodosAsync();
 var user = users.FirstOrDefault(u => string.Equals(u.Email, req.NombreUsuarioOrEmail, StringComparison.OrdinalIgnoreCase) || string.Equals(u.NombreUsuario, req.NombreUsuarioOrEmail, StringComparison.OrdinalIgnoreCase));
 if (user == null) return Unauthorized(new { message = "Usuario o contraseña inválidos" });
 // Comparación directa, ya que las contraseñas se almacenan en texto plano por ahora
 if (!string.Equals(user.Contrasena, req.Password)) return Unauthorized(new { message = "Usuario o contraseña inválidos" });
 var token = _jwt.CreateToken(user);
 var res = new LoginResponse(user.Id, user.RolId, user.NombreUsuario, user.Email, token);
 return Ok(res);
 }
 }
}
