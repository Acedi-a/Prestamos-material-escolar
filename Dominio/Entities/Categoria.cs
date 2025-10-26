using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entities
{
 public class Categoria
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 [MaxLength(255)]
 public string NombreCategoria { get; set; } = string.Empty;
 
 public string? Descripcion { get; set; }
 
 // Navigation
 public ICollection<Material>? Materiales { get; set; }
 }
}
