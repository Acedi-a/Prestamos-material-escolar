using System.Security.Cryptography;
using System.Text;
using Dominio.Interfaces;

namespace Infraestructura.Servicios
{
 public class PasswordHasher : IPasswordHasher
 {
 public string Hash(string password)
 {
 using var sha = SHA256.Create();
 var bytes = Encoding.UTF8.GetBytes(password);
 var hash = sha.ComputeHash(bytes);
 return Convert.ToHexString(hash);
 }

 public bool Verify(string password, string passwordHash)
 {
 return string.Equals(Hash(password), passwordHash, StringComparison.OrdinalIgnoreCase);
 }
 }
}
