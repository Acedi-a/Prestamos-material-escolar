using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Docente
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(150)]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string CedulaIdentidad { get; set; } = string.Empty;
        
        // Navigation
        public ICollection<Solicitud>? Solicitudes { get; set; }
    }
}
