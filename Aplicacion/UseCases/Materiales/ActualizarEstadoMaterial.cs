using System.Threading.Tasks;
using Dominio.Interfaces;

namespace Aplication.UseCases.Materiales
{
 public class ActualizarEstadoMaterial
 {
 private readonly IMaterialRepositorio _materiales;
 public ActualizarEstadoMaterial(IMaterialRepositorio materiales) { _materiales = materiales; }

 public async Task EjecutarAsync(int materialId, string nuevoEstado)
 {
 var material = await _materiales.ObtenerPorIdAsync(materialId) ?? throw new System.ArgumentException("Material no existe");
 if (string.IsNullOrWhiteSpace(nuevoEstado)) throw new System.ArgumentException("Estado inválido");
 material.Estado = nuevoEstado;
 await _materiales.ActualizarAsync(material);
 }
 }
}
