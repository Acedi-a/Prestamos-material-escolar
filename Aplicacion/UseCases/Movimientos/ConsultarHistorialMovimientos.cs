using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Movimientos
{
 public class ConsultarHistorialMovimientos
 {
 private readonly IMovimientoRepositorio _movimientos;
 public ConsultarHistorialMovimientos(IMovimientoRepositorio movimientos) { _movimientos = movimientos; }

 public async Task<IEnumerable<Movimiento>> EjecutarAsync(int? materialId = null)
 {
 var lista = await _movimientos.ListarTodosAsync();
 if (materialId.HasValue) lista = lista.Where(m => m.MaterialId == materialId.Value);
 return lista;
 }
 }
}
