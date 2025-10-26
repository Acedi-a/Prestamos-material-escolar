using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface ISolicitudRepositorio
 {
 Task<Solicitud?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Solicitud>> ListarTodosAsync();
 Task CrearAsync(Solicitud solicitud);
 Task ActualizarAsync(Solicitud solicitud);
 Task EliminarAsync(int id);
 }
}
