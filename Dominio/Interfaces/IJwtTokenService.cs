using Dominio.Entities;

namespace Dominio.Interfaces
{
 public interface IJwtTokenService
 {
 string CreateToken(Usuario usuario);
 }
}
