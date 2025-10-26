using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Reparaciones
{
 public class ConsultarHistorialReparaciones
 {
 private readonly IHistorialReparacionRepositorio _historial;
 public ConsultarHistorialReparaciones(IHistorialReparacionRepositorio historial) { _historial = historial; }

 public async Task<IEnumerable<HistorialReparacion>> EjecutarAsync(int? materialId = null)
 {
 var lista = await _historial.ListarTodosAsync();
 if (materialId.HasValue) lista = lista.Where(h => h.MaterialId == materialId.Value);
 return lista;
 }
 }
}
