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

 public async Task<int> EjecutarAsync(int materialId, System.DateTime fechaEnvio, string descripcionFalla, decimal? costo)
 {
 var material = await _materiales.ObtenerPorIdAsync(materialId) ?? throw new System.ArgumentException("Material no existe");

 var reparacion = new HistorialReparacion { MaterialId = material.Id, FechaEnvio = fechaEnvio, DescripcionFalla = descripcionFalla, Costo = costo };
 await _historial.CrearAsync(reparacion);

 material.Estado = "En Mantenimiento";
 await _materiales.ActualizarAsync(material);

 await _movimientos.CrearAsync(new Movimiento { MaterialId = material.Id, TipoMovimiento = "Ajuste/Salida a Reparación", FechaMovimiento = System.DateTime.UtcNow, Cantidad =0 });

 return reparacion.Id;
 }
 }
}
