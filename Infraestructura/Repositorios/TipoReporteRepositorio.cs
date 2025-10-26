using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class TipoReporteRepositorio : ITipoReporteRepositorio
 {
 private readonly AppDbContext _context;
 public TipoReporteRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<TipoReporte?> ObtenerPorIdAsync(int id) => await _context.TiposReportes.FindAsync(id);
 public async Task<IEnumerable<TipoReporte>> ListarTodosAsync() => await _context.TiposReportes.ToListAsync();
 public async Task CrearAsync(TipoReporte tipoReporte) { await _context.TiposReportes.AddAsync(tipoReporte); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(TipoReporte tipoReporte) { _context.TiposReportes.Update(tipoReporte); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.TiposReportes.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
