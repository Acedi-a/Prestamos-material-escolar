using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class Solicitud
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int DocenteId { get; set; }
 
 [ForeignKey(nameof(DocenteId))]
 public Docente? Docente { get; set; }
 
 [Required]
 public DateTime FechaSolicitud { get; set; }
 
 [Required]
 [MaxLength(100)]
 public string EstadoSolicitud { get; set; } = string.Empty;
 
 // Navigation
 public ICollection<SolicitudDetalle>? Detalles { get; set; }
 public Prestamo? Prestamo { get; set; }
 }
}
