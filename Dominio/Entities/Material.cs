using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class Material
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int CategoriaId { get; set; }
 
 [ForeignKey(nameof(CategoriaId))]
 public Categoria? Categoria { get; set; }
 
 [Required]
 [MaxLength(255)]
 public string NombreMaterial { get; set; } = string.Empty;
 
 public string? Descripcion { get; set; }
 
 [Required]
 public int CantidadTotal { get; set; }
 
 [Required]
 public int CantidadDisponible { get; set; }
 
 [Required]
 [MaxLength(100)]
 public string Estado { get; set; } = string.Empty;
 
 // Navigation
 public ICollection<HistorialReparacion>? HistorialReparaciones { get; set; }
 public ICollection<SolicitudDetalle>? SolicitudesDetalle { get; set; }
 public ICollection<PrestamoDetalle>? PrestamosDetalle { get; set; }
 public ICollection<Movimiento>? Movimientos { get; set; }
 }
}
