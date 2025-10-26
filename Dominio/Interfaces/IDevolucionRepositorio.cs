using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IDevolucionRepositorio
 {
 Task<Devolucion?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Devolucion>> ListarTodosAsync();
 Task CrearAsync(Devolucion devolucion);
 Task ActualizarAsync(Devolucion devolucion);
 Task EliminarAsync(int id);
 }
}
