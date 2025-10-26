namespace Aplication.DTOs
{
 public class MaterialDTO
 {
 public int Id { get; set; }
 public int CategoriaId { get; set; }
 public string NombreMaterial { get; set; } = string.Empty;
 public string? Descripcion { get; set; }
 public int CantidadTotal { get; set; }
 public int CantidadDisponible { get; set; }
 public string Estado { get; set; } = string.Empty;
 }
}
