using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearDocente
    {
        private readonly IDocenteRepositorio _docenteRepositorio;
        public CrearDocente(IDocenteRepositorio docenteRepositorio)
        {
            _docenteRepositorio = docenteRepositorio;
        }   

        public async Task EjecutarAsync (Docente docente)
        {
            ValidarDocente(docente);

            await _docenteRepositorio.CrearAsync(docente);
        }

        private void ValidarDocente(Docente docente)
        {
            if (string.IsNullOrEmpty(docente.Nombre) || docente.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre del docente es inválido. Debe tener al menos 3 caracteres.");
            }
            if (string.IsNullOrEmpty(docente.Apellido) || docente.Apellido.Length < 3)
            {
                throw new ArgumentException("El apellido del docente es inválido. Debe tener al menos 3 caracteres.");
            }
            if (string.IsNullOrEmpty(docente.Correo) || !docente.Correo.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico del docente es inválido.");
            }
        }
    }
}
