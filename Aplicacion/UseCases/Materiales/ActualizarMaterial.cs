using System.Threading.Tasks;
using Dominio.Interfaces;

namespace Aplication.UseCases.Materiales
{
 public class ActualizarMaterial
 {
 private readonly IMaterialRepositorio _materiales;
 public ActualizarMaterial(IMaterialRepositorio materiales) { _materiales = materiales; }

 public async Task EjecutarAsync(int materialId, int categoriaId, string nombre, string? descripcion, int cantidadTotal, int cantidadDisponible, string estado)
 {
 var material = await _materiales.ObtenerPorIdAsync(materialId) ?? throw new System.ArgumentException("Material no existe");
 if (string.IsNullOrWhiteSpace(nombre)) throw new System.ArgumentException("Nombre inv�lido");
 if (cantidadTotal <0) throw new System.ArgumentException("Cantidad total inv�lida");
 if (cantidadDisponible <0 || cantidadDisponible > cantidadTotal) throw new System.ArgumentException("Cantidad disponible inv�lida");
 if (string.IsNullOrWhiteSpace(estado)) throw new System.ArgumentException("Estado inv�lido");

 material.CategoriaId = categoriaId;
 material.NombreMaterial = nombre;
 material.Descripcion = descripcion;
 material.CantidadTotal = cantidadTotal;
 material.CantidadDisponible = cantidadDisponible;
 material.Estado = estado;

 await _materiales.ActualizarAsync(material);
 }
 }
}
