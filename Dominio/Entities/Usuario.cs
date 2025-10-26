using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class Usuario
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int RolId { get; set; }
 
 [ForeignKey(nameof(RolId))]
 public Rol? Rol { get; set; }
 
 [Required]
 [MaxLength(100)]
 public string NombreUsuario { get; set; } = string.Empty;
 
 [Required]
 [MaxLength(255)]
 public string Email { get; set; } = string.Empty;
 
 [Required]
 [MaxLength(255)]
 public string Contrasena { get; set; } = string.Empty; // Hash
 
 // Navigation
 public Docente? Docente { get; set; }
 public ICollection<RegistroReporte>? RegistroReportes { get; set; }
 }
}
