using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Materiales
{
 public class RegistrarMaterial
 {
 private readonly IMaterialRepositorio _materiales;
 private readonly ICategoriaRepositorio _categorias;

 public RegistrarMaterial(IMaterialRepositorio materiales, ICategoriaRepositorio categorias)
 {
 _materiales = materiales;
 _categorias = categorias;
 }

 public async Task<int> EjecutarAsync(int categoriaId, string nombre, string? descripcion, int cantidadInicial, string estado)
 {
 if (string.IsNullOrWhiteSpace(nombre)) throw new System.ArgumentException("Nombre material requerido");
 if (cantidadInicial <0) throw new System.ArgumentException("Cantidad inicial inválida");
 if (string.IsNullOrWhiteSpace(estado)) throw new System.ArgumentException("Estado requerido");

 _ = await _categorias.ObtenerPorIdAsync(categoriaId) ?? throw new System.ArgumentException("Categoría no existe");
 var material = new Material
 {
 CategoriaId = categoriaId,
 NombreMaterial = nombre,
 Descripcion = descripcion,
 CantidadTotal = cantidadInicial,
 CantidadDisponible = cantidadInicial,
 Estado = estado
 };
 await _materiales.CrearAsync(material);
 return material.Id;
 }
 }
}
