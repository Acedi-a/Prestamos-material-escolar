using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
 public class RegistroReporteRepositorio : IRegistroReporteRepositorio
 {
 private readonly AppDbContext _context;
 public RegistroReporteRepositorio(AppDbContext context) { _context = context; }

 public async Task<RegistroReporte?> ObtenerPorIdAsync(int id) => await _context.RegistroReportes.FindAsync(id);
 public async Task<IEnumerable<RegistroReporte>> ListarTodosAsync() => await _context.RegistroReportes.ToListAsync();
 public async Task CrearAsync(RegistroReporte registro) { await _context.RegistroReportes.AddAsync(registro); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(RegistroReporte registro) { _context.RegistroReportes.Update(registro); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.RegistroReportes.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
