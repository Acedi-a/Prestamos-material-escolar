using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class HistorialReparacion
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int MaterialId { get; set; }
 
 [ForeignKey(nameof(MaterialId))]
 public Material? Material { get; set; }
 
 [Required]
 public DateTime FechaEnvio { get; set; }
 
 public DateTime? FechaRetorno { get; set; }
 
 [Required]
 public string DescripcionFalla { get; set; } = string.Empty;
 
 public decimal? Costo { get; set; }
 }
}
