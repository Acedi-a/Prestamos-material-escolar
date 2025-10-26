using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Solicitudes
{
 public class SolicitarMaterial
 {
 private readonly ISolicitudRepositorio _solicitudes;
 private readonly ISolicitudDetalleRepositorio _detalles;
 private readonly IMaterialRepositorio _materiales;

 public SolicitarMaterial(ISolicitudRepositorio solicitudes, ISolicitudDetalleRepositorio detalles, IMaterialRepositorio materiales)
 {
 _solicitudes = solicitudes;
 _detalles = detalles;
 _materiales = materiales;
 }

 public async Task<int> EjecutarAsync(int docenteId, IEnumerable<(int materialId, int cantidad)> items)
 {
 if (items == null || !items.Any()) throw new System.ArgumentException("Debe especificar materiales");

 var solicitud = new Solicitud
 {
 DocenteId = docenteId,
 FechaSolicitud = System.DateTime.UtcNow,
 EstadoSolicitud = "Pendiente"
 };
 await _solicitudes.CrearAsync(solicitud);

 foreach (var it in items)
 {
 var material = await _materiales.ObtenerPorIdAsync(it.materialId) ?? throw new System.ArgumentException($"Material {it.materialId} no existe");
 if (it.cantidad <=0) throw new System.ArgumentException("Cantidad inválida");
 if (it.cantidad > material.CantidadDisponible)
	throw new System.ArgumentException($"Stock insuficiente para '{material.NombreMaterial}'. Solicitados: {it.cantidad}, Disponibles: {material.CantidadDisponible}");
var det = new SolicitudDetalle { SolicitudId = solicitud.Id, MaterialId = material.Id, CantidadSolicitada = it.cantidad };
 await _detalles.CrearAsync(det);
 }

 return solicitud.Id;
 }
 }
}
