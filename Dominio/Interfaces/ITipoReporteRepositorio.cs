using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface ITipoReporteRepositorio
 {
 Task<TipoReporte?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<TipoReporte>> ListarTodosAsync();
 Task CrearAsync(TipoReporte tipoReporte);
 Task ActualizarAsync(TipoReporte tipoReporte);
 Task EliminarAsync(int id);
 }
}
