using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Devoluciones
{
 public class RegistrarDevolucion
 {
 private readonly IDevolucionRepositorio _devoluciones;
 private readonly IPrestamoRepositorio _prestamos;
 private readonly IPrestamoDetalleRepositorio _detalles;
 private readonly IMaterialRepositorio _materiales;
 private readonly IMovimientoRepositorio _movimientos;

 public RegistrarDevolucion(IDevolucionRepositorio devoluciones, IPrestamoRepositorio prestamos, IPrestamoDetalleRepositorio detalles, IMaterialRepositorio materiales, IMovimientoRepositorio movimientos)
 {
 _devoluciones = devoluciones;
 _prestamos = prestamos;
 _detalles = detalles;
 _materiales = materiales;
 _movimientos = movimientos;
 }

 public async Task<int> EjecutarAsync(int prestamoId, string? observaciones)
 {
 var prestamo = await _prestamos.ObtenerPorIdAsync(prestamoId) ?? throw new System.ArgumentException("Préstamo no existe");
 var devolucion = new Devolucion { PrestamoId = prestamo.Id, FechaDevolucion = System.DateTime.UtcNow, Observaciones = observaciones };
 await _devoluciones.CrearAsync(devolucion);

 // Sumar stock por cada detalle
 var detalles = (await _detalles.ListarTodosAsync()).Where(d => d.PrestamoId == prestamo.Id).ToList();
 foreach (var det in detalles)
 {
 var material = await _materiales.ObtenerPorIdAsync(det.MaterialId) ?? throw new System.ArgumentException($"Material {det.MaterialId} no existe");
 material.CantidadDisponible += det.CantidadPrestada;
 await _materiales.ActualizarAsync(material);
 await _movimientos.CrearAsync(new Movimiento { MaterialId = det.MaterialId, TipoMovimiento = "Entrada", FechaMovimiento = System.DateTime.UtcNow, Cantidad = det.CantidadPrestada, PrestamoId = prestamo.Id });
 }

 prestamo.EstadoPrestamo = "Devuelto";
 await _prestamos.ActualizarAsync(prestamo);
 return devolucion.Id;
 }
 }
}
