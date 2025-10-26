using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class Devolucion
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int PrestamoId { get; set; }
 
 [ForeignKey(nameof(PrestamoId))]
 public Prestamo? Prestamo { get; set; }
 
 [Required]
 public DateTime FechaDevolucion { get; set; }
 
 public string? Observaciones { get; set; }
 }
}
