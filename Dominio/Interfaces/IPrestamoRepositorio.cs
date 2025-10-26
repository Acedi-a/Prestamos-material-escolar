using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IPrestamoRepositorio
 {
 Task<Prestamo?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Prestamo>> ListarTodosAsync();
 Task CrearAsync(Prestamo prestamo);
 Task ActualizarAsync(Prestamo prestamo);
 Task EliminarAsync(int id);
 }
}
