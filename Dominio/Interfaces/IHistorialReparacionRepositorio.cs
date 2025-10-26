using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IHistorialReparacionRepositorio
 {
 Task<HistorialReparacion?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<HistorialReparacion>> ListarTodosAsync();
 Task CrearAsync(HistorialReparacion reparacion);
 Task ActualizarAsync(HistorialReparacion reparacion);
 Task EliminarAsync(int id);
 }
}
