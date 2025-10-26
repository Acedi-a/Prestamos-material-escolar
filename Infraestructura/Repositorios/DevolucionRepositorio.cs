using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class DevolucionRepositorio : IDevolucionRepositorio
 {
 private readonly AppDbContext _context;
 public DevolucionRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Devolucion?> ObtenerPorIdAsync(int id) => await _context.Devoluciones.FindAsync(id);
 public async Task<IEnumerable<Devolucion>> ListarTodosAsync() => await _context.Devoluciones.ToListAsync();
 public async Task CrearAsync(Devolucion devolucion) { await _context.Devoluciones.AddAsync(devolucion); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Devolucion devolucion) { _context.Devoluciones.Update(devolucion); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.Devoluciones.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
