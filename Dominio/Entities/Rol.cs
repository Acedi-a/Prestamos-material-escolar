using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entities
{
 public class Rol
 {
 [Key]
 public int Id { get; set; }

 [Required]
 [MaxLength(100)]
 public string NombreRol { get; set; } = string.Empty;

 // Navigation
 public ICollection<Usuario>? Usuarios { get; set; }
 }
}
