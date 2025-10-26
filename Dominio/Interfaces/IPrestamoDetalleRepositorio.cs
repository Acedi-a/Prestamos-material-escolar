using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IPrestamoDetalleRepositorio
 {
 Task<PrestamoDetalle?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<PrestamoDetalle>> ListarTodosAsync();
 Task CrearAsync(PrestamoDetalle detalle);
 Task ActualizarAsync(PrestamoDetalle detalle);
 Task EliminarAsync(int id);
 }
}
