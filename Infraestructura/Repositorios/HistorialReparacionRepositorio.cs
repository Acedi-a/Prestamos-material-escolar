using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class HistorialReparacionRepositorio : IHistorialReparacionRepositorio
 {
 private readonly AppDbContext _context;
 public HistorialReparacionRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<HistorialReparacion?> ObtenerPorIdAsync(int id) => await _context.HistorialReparaciones.FindAsync(id);
 public async Task<IEnumerable<HistorialReparacion>> ListarTodosAsync() => await _context.HistorialReparaciones.ToListAsync();
 public async Task CrearAsync(HistorialReparacion reparacion) { await _context.HistorialReparaciones.AddAsync(reparacion); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(HistorialReparacion reparacion) { _context.HistorialReparaciones.Update(reparacion); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.HistorialReparaciones.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
