using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface ICategoriaRepositorio
 {
 Task<Categoria?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Categoria>> ListarTodosAsync();
 Task CrearAsync(Categoria categoria);
 Task ActualizarAsync(Categoria categoria);
 Task EliminarAsync(int id);
 }
}
