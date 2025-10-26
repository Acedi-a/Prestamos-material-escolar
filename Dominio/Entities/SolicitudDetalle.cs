using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class SolicitudDetalle
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int SolicitudId { get; set; }
 
 [ForeignKey(nameof(SolicitudId))]
 public Solicitud? Solicitud { get; set; }
 
 [Required]
 public int MaterialId { get; set; }
 
 [ForeignKey(nameof(MaterialId))]
 public Material? Material { get; set; }
 
 [Required]
 public int CantidadSolicitada { get; set; }
 }
}
