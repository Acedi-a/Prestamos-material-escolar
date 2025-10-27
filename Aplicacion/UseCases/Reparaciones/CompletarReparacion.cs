using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Reparaciones
{
 public class CompletarReparacion
 {
 private readonly IHistorialReparacionRepositorio _historial;
 private readonly IMaterialRepositorio _materiales;
 private readonly IMovimientoRepositorio _movimientos;

 public CompletarReparacion(IHistorialReparacionRepositorio historial, IMaterialRepositorio materiales, IMovimientoRepositorio movimientos)
 {
 _historial = historial; _materiales = materiales; _movimientos = movimientos;
 }

 public async Task EjecutarAsync(int reparacionId, System.DateTime? fechaRetorno = null)
 {
 var reparacion = await _historial.ObtenerPorIdAsync(reparacionId) ?? throw new System.ArgumentException("Reparación no existe");
 if (reparacion.FechaRetorno != null) throw new System.InvalidOperationException("La reparación ya fue completada");
 var material = await _materiales.ObtenerPorIdAsync(reparacion.MaterialId) ?? throw new System.ArgumentException("Material no existe");

 // Sumar stock de vuelta y marcar disponible
 material.CantidadDisponible += reparacion.Cantidad;
 material.Estado = "Disponible";
 await _materiales.ActualizarAsync(material);

 // Actualizar historial con la fecha de retorno
 reparacion.FechaRetorno = fechaRetorno ?? System.DateTime.UtcNow;
 await _historial.ActualizarAsync(reparacion);

 // Registrar movimiento de entrada por reparación
 await _movimientos.CrearAsync(new Movimiento
 {
 MaterialId = reparacion.MaterialId,
 TipoMovimiento = "Entrada por Reparación",
 FechaMovimiento = System.DateTime.UtcNow,
 Cantidad = reparacion.Cantidad
 });
 }
 }
}
