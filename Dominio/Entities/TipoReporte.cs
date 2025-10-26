using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entities
{
 public class TipoReporte
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 [MaxLength(255)]
 public string NombreReporte { get; set; } = string.Empty;
 
 public ICollection<RegistroReporte>? RegistroReportes { get; set; }
 }
}
