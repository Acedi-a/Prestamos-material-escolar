using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Prestamos
{
 public class RegistrarPrestamo
 {
 private readonly IPrestamoRepositorio _prestamos;
 private readonly IPrestamoDetalleRepositorio _detalles;
 private readonly ISolicitudRepositorio _solicitudes;
 private readonly ISolicitudDetalleRepositorio _solSolicitudDet;
 private readonly IMaterialRepositorio _materiales;
 private readonly IMovimientoRepositorio _movimientos;

 public RegistrarPrestamo(IPrestamoRepositorio prestamos, IPrestamoDetalleRepositorio detalles, ISolicitudRepositorio solicitudes, ISolicitudDetalleRepositorio solDetalles, IMaterialRepositorio materiales, IMovimientoRepositorio movimientos)
 {
 _prestamos = prestamos;
 _detalles = detalles;
 _solicitudes = solicitudes;
 _solSolicitudDet = solDetalles;
 _materiales = materiales;
 _movimientos = movimientos;
 }

 public async Task<int> EjecutarAsync(int solicitudId, System.DateTime fechaPrevista)
 {
 var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId) ?? throw new System.ArgumentException("Solicitud no existe");
 if (!string.Equals(solicitud.EstadoSolicitud, "Aprobada", System.StringComparison.OrdinalIgnoreCase))
 throw new System.InvalidOperationException("La solicitud debe estar Aprobada");

 var prestamo = new Prestamo
 {
 SolicitudId = solicitud.Id,
 FechaPrestamo = System.DateTime.UtcNow,
 FechaDevolucionPrevista = fechaPrevista,
 EstadoPrestamo = "Activo"
 };
 await _prestamos.CrearAsync(prestamo);

 // cargar detalles de la solicitud
 var detallesSolicitud = (await _solSolicitudDet.ListarTodosAsync()).Where(d => d.SolicitudId == solicitud.Id).ToList();
 if (detallesSolicitud.Count ==0) throw new System.InvalidOperationException("La solicitud no tiene detalles");

 foreach (var sd in detallesSolicitud)
 {
 var material = await _materiales.ObtenerPorIdAsync(sd.MaterialId) ?? throw new System.ArgumentException($"Material {sd.MaterialId} no existe");
 if (material.CantidadDisponible < sd.CantidadSolicitada) throw new System.InvalidOperationException($"Material {material.NombreMaterial} sin stock suficiente");
 material.CantidadDisponible -= sd.CantidadSolicitada;
 await _materiales.ActualizarAsync(material);
 var detalle = new PrestamoDetalle { PrestamoId = prestamo.Id, MaterialId = sd.MaterialId, CantidadPrestada = sd.CantidadSolicitada };
 await _detalles.CrearAsync(detalle);
 await _movimientos.CrearAsync(new Movimiento { MaterialId = sd.MaterialId, TipoMovimiento = "Salida por Préstamo", FechaMovimiento = System.DateTime.UtcNow, Cantidad = sd.CantidadSolicitada, PrestamoId = prestamo.Id });
 }

 return prestamo.Id;
 }
 }
}
