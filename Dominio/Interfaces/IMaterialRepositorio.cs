using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IMaterialRepositorio
 {
 Task<Material?> ObtenerPorIdAsync(int id);
 Task<IEnumerable<Material>> ListarTodosAsync();
 Task CrearAsync(Material material);
 Task ActualizarAsync(Material material);
 Task EliminarAsync(int id);
 // Devuelve (prestados, enReparacion) para un material dado
 Task<(int Prestados, int EnReparacion)> ContarPrestadosYEnReparacionPorMaterialAsync(int materialId);
 }
}
