using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class RegistroReporte
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int TipoReporteId { get; set; }
 
 [ForeignKey(nameof(TipoReporteId))]
 public TipoReporte? TipoReporte { get; set; }
 
 [Required]
 public int UsuarioId { get; set; }
 
 [ForeignKey(nameof(UsuarioId))]
 public Usuario? Usuario { get; set; }
 
 public int? MovimientoId { get; set; }
 
 [ForeignKey(nameof(MovimientoId))]
 public Movimiento? Movimiento { get; set; }
 
 [Required]
 public DateTime FechaGeneracion { get; set; }
 
 public string? Parametros { get; set; }
 }
}
