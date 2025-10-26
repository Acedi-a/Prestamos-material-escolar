using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Docente
    {

        [Key]
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Descripcion { get; set; }
        public string? Correo { get; set; }
        public ICollection<Curso>? cursos { get; set; } 



    }
}
