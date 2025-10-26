using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IRegistroReporteRepositorio
 {
 Task<RegistroReporte?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<RegistroReporte>> ListarTodosAsync();
 Task CrearAsync(RegistroReporte registro);
 Task ActualizarAsync(RegistroReporte registro);
 Task EliminarAsync(int id);
 }
}
