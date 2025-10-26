using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class MovimientoRepositorio : IMovimientoRepositorio
 {
 private readonly AppDbContext _context;
 public MovimientoRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Movimiento?> ObtenerPorIdAsync(int id) => await _context.Movimientos
 .Include(m => m.Material)
 .FirstOrDefaultAsync(m => m.Id == id);
 
 public async Task<IEnumerable<Movimiento>> ListarTodosAsync() => await _context.Movimientos
 .Include(m => m.Material)
 .ToListAsync();
 
 public async Task CrearAsync(Movimiento movimiento) { await _context.Movimientos.AddAsync(movimiento); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Movimiento movimiento) { _context.Movimientos.Update(movimiento); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.Movimientos.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
