using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class DocenteDTO
    {
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Descripcion { get; set; }
        public string? Correo { get; set; }
    }
}
