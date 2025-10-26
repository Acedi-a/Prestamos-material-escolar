using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class PrestamoRepositorio : IPrestamoRepositorio
 {
 private readonly AppDbContext _context;
 public PrestamoRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Prestamo?> ObtenerPorIdAsync(int id) => await _context.Prestamos.FindAsync(id);
 public async Task<IEnumerable<Prestamo>> ListarTodosAsync() => await _context.Prestamos.ToListAsync();
 public async Task CrearAsync(Prestamo prestamo) { await _context.Prestamos.AddAsync(prestamo); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Prestamo prestamo) { _context.Prestamos.Update(prestamo); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.Prestamos.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
