using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface ISolicitudDetalleRepositorio
 {
 Task<SolicitudDetalle?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<SolicitudDetalle>> ListarTodosAsync();
 Task CrearAsync(SolicitudDetalle detalle);
 Task ActualizarAsync(SolicitudDetalle detalle);
 Task EliminarAsync(int id);
 }
}
