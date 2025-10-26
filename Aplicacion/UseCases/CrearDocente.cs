using Dominio.Entities;
using Dominio.Interfaces;
using System;
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

        public async Task EjecutarAsync(Docente docente)
        {
            ValidarDocente(docente);
            await _docenteRepositorio.CrearAsync(docente);
        }

        private void ValidarDocente(Docente docente)
        {
            if (string.IsNullOrEmpty(docente.Nombre) || docente.Nombre.Length <3)
            {
                throw new ArgumentException("El nombre del docente es inválido. Debe tener al menos3 caracteres.");
            }
            if (string.IsNullOrEmpty(docente.Apellido) || docente.Apellido.Length <3)
            {
                throw new ArgumentException("El apellido del docente es inválido. Debe tener al menos3 caracteres.");
            }
            // Validación básica: el correo está en Usuario.Email, no en Docente
            if (docente.Usuario == null || string.IsNullOrEmpty(docente.Usuario.Email) || !docente.Usuario.Email.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico del docente es inválido.");
            }
        }
    }
}
