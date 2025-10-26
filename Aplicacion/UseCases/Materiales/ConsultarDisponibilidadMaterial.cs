using System.Threading.Tasks;
using Dominio.Interfaces;

namespace Aplication.UseCases.Materiales
{
 public class ConsultarDisponibilidadMaterial
 {
 private readonly IMaterialRepositorio _materiales;
 public ConsultarDisponibilidadMaterial(IMaterialRepositorio materiales) { _materiales = materiales; }

 public async Task<(int cantidadDisponible, string estado)> EjecutarAsync(int materialId)
 {
 var material = await _materiales.ObtenerPorIdAsync(materialId) ?? throw new System.ArgumentException("Material no existe");
 return (material.CantidadDisponible, material.Estado);
 }
 }
}
