using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
 public class PrestamoDetalle
 {
 [Key]
 public int Id { get; set; }
 
 [Required]
 public int PrestamoId { get; set; }
 
 [ForeignKey(nameof(PrestamoId))]
 public Prestamo? Prestamo { get; set; }
 
 [Required]
 public int MaterialId { get; set; }
 
 [ForeignKey(nameof(MaterialId))]
 public Material? Material { get; set; }
 
 [Required]
 public int CantidadPrestada { get; set; }
 }
}
