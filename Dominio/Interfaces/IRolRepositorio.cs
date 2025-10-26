using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IRolRepositorio
 {
 Task<Rol?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Rol>> ListarTodosAsync();
 Task CrearAsync(Rol rol);
 Task ActualizarAsync(Rol rol);
 Task EliminarAsync(int id);
 }
}
