using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class DocenteRepositorio: IDocenteRepositorio
    {
        private readonly AppDbContext _context;
        public DocenteRepositorio (AppDbContext context)
        {
            _context = context;
        }

        public async Task<Docente?> ObtenerPorIdAsync(Guid id)
        { 
            return await _context.Docentes.FindAsync(id);
        }

        public async Task<IEnumerable<Docente>> ListarTodosAsync()
        {
            return await _context.Docentes.ToListAsync();
        }

        public async Task CrearAsync(Docente docente)
        {
            await _context.Docentes.AddAsync(docente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Docente docente)
        {
            _context.Docentes.Update(docente);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Guid id)
        {
            var docente = await ObtenerPorIdAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
                await _context.SaveChangesAsync();
            }
        }

    }
}
