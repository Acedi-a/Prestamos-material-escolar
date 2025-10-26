using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class Prestamo
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int SolicitudId { get; set; }
 
 [ForeignKey(nameof(SolicitudId))]
 public Solicitud? Solicitud { get; set; }
 
 [Required]
 public DateTime FechaPrestamo { get; set; }
 
 [Required]
 public DateTime FechaDevolucionPrevista { get; set; }
 
 [Required]
 [MaxLength(100)]
 public string EstadoPrestamo { get; set; } = string.Empty;
 
 // Navigation
 public ICollection<PrestamoDetalle>? Detalles { get; set; }
 public ICollection<Devolucion>? Devoluciones { get; set; }
 public ICollection<Movimiento>? Movimientos { get; set; }
 }
}
