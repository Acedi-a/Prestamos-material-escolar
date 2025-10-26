using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Reparaciones
{
 public class RegistrarEnvioAReparacion
 {
 private readonly IHistorialReparacionRepositorio _historial;
 private readonly IMaterialRepositorio _materiales;
 private readonly IMovimientoRepositorio _movimientos;

 public RegistrarEnvioAReparacion(IHistorialReparacionRepositorio historial, IMaterialRepositorio materiales, IMovimientoRepositorio movimientos)
 {
 _historial = historial; _materiales = materiales; _movimientos = movimientos;
 }

 public async Task<int> EjecutarAsync(int materialId, System.DateTime fechaEnvio, string descripcionFalla, decimal? costo, int cantidad)
 {
 var material = await _materiales.ObtenerPorIdAsync(materialId) ?? throw new System.ArgumentException("Material no existe");
 if (cantidad <=0) throw new System.ArgumentException("La cantidad debe ser mayor a cero");
 if (material.CantidadDisponible < cantidad) throw new System.InvalidOperationException("Stock disponible insuficiente para enviar a reparación");

 // Registrar historial
 var reparacion = new HistorialReparacion
 {
 MaterialId = material.Id,
 FechaEnvio = fechaEnvio,
 DescripcionFalla = descripcionFalla,
 Costo = costo,
 Cantidad = cantidad
 };
 await _historial.CrearAsync(reparacion);


 // Actualizar material: restar disponibilidad y marcar estado
 material.CantidadDisponible -= cantidad;
 //material.Estado = "En Mantenimiento";
 await _materiales.ActualizarAsync(material);


 // Registrar movimiento
 await _movimientos.CrearAsync(new Movimiento
 {
 MaterialId = material.Id,
 TipoMovimiento = "Salida a Reparación",
 FechaMovimiento = System.DateTime.UtcNow,
 Cantidad = cantidad
 });

 return reparacion.Id;
 }
 }
}
