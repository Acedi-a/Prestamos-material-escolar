using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class PrestamoDetalleRepositorio : IPrestamoDetalleRepositorio
 {
 private readonly AppDbContext _context;
 public PrestamoDetalleRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<PrestamoDetalle?> ObtenerPorIdAsync(int id) => await _context.PrestamosDetalle.FindAsync(id);
 public async Task<IEnumerable<PrestamoDetalle>> ListarTodosAsync() => await _context.PrestamosDetalle.ToListAsync();
 public async Task CrearAsync(PrestamoDetalle detalle) { await _context.PrestamosDetalle.AddAsync(detalle); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(PrestamoDetalle detalle) { _context.PrestamosDetalle.Update(detalle); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.PrestamosDetalle.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
