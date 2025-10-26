using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infraestructura.Servicios
{
 public class JwtTokenService : IJwtTokenService
 {
 private readonly IConfiguration _config;
 public JwtTokenService(IConfiguration config) { _config = config; }

 public string CreateToken(Usuario usuario)
 {
 var keyCfg = _config["Jwt:Key"] ?? "dev-secret-key-please-change-0123456789"; // >=32 chars
 var issuer = _config["Jwt:Issuer"] ?? "app";
 var audience = _config["Jwt:Audience"] ?? "app";
 var claims = new List<Claim>
 {
 new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
 new Claim(ClaimTypes.Name, usuario.NombreUsuario),
 new Claim(ClaimTypes.Email, usuario.Email),
 new Claim(ClaimTypes.Role, usuario.RolId.ToString())
 };
 var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyCfg));
 var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(4), signingCredentials: creds);
 return new JwtSecurityTokenHandler().WriteToken(token);
 }
 }
}
