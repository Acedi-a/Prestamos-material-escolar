using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class Movimiento
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int MaterialId { get; set; }
 
 [ForeignKey(nameof(MaterialId))]
 public Material? Material { get; set; }
 
 [Required]
 [MaxLength(100)]
 public string TipoMovimiento { get; set; } = string.Empty;
 
 [Required]
 public DateTime FechaMovimiento { get; set; }
 
 [Required]
 public int Cantidad { get; set; }
 
 public int? PrestamoId { get; set; }
 
 [ForeignKey(nameof(PrestamoId))]
 public Prestamo? Prestamo { get; set; }
 }
}
