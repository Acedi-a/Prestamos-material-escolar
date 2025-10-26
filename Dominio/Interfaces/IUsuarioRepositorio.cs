using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IUsuarioRepositorio
 {
 Task<Usuario?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Usuario>> ListarTodosAsync();
 Task CrearAsync(Usuario usuario);
 Task ActualizarAsync(Usuario usuario);
 Task EliminarAsync(int id);
 }
}
