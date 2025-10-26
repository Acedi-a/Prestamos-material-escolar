using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IMovimientoRepositorio
 {
 Task<Movimiento?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Movimiento>> ListarTodosAsync();
 Task CrearAsync(Movimiento movimiento);
 Task ActualizarAsync(Movimiento movimiento);
 Task EliminarAsync(int id);
 }
}
