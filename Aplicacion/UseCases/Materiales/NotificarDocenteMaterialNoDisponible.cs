using System.Linq;
using System.Threading.Tasks;
using Dominio.Interfaces;

namespace Aplication.UseCases.Materiales
{
 public class NotificarDocenteMaterialNoDisponible
 {
 private readonly ISolicitudRepositorio _solicitudes;
 private readonly ISolicitudDetalleRepositorio _detalles;
 private readonly IMaterialRepositorio _materiales;
 private readonly INotificacionServicio _notifier;

 public NotificarDocenteMaterialNoDisponible(ISolicitudRepositorio solicitudes, ISolicitudDetalleRepositorio detalles, IMaterialRepositorio materiales, INotificacionServicio notifier)
 {
 _solicitudes = solicitudes; _detalles = detalles; _materiales = materiales; _notifier = notifier;
 }

 public async Task EjecutarAsync(int solicitudId)
 {
 var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId) ?? throw new System.ArgumentException("Solicitud no existe");
 var detalles = (await _detalles.ListarTodosAsync()).Where(d => d.SolicitudId == solicitud.Id).ToList();
 foreach (var det in detalles)
 {
 var material = await _materiales.ObtenerPorIdAsync(det.MaterialId);
 if (material == null) continue;
 if (material.Estado != "Disponible" || material.CantidadDisponible < det.CantidadSolicitada)
 {
 var asunto = "Material no disponible";
 var mensaje = $"El material '{material.NombreMaterial}' no está disponible o está en reparación.";
 await _notifier.EnviarAsync(solicitud.DocenteId, asunto, mensaje);
 }
 }
 }
 }
}
