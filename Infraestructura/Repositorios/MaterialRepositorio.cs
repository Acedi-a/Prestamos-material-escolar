using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class MaterialRepositorio : IMaterialRepositorio
 {
 private readonly AppDbContext _context;
 public MaterialRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Material?> ObtenerPorIdAsync(int id) => await _context.Materiales.FindAsync(id);
 public async Task<IEnumerable<Material>> ListarTodosAsync() => await _context.Materiales.ToListAsync();
 public async Task CrearAsync(Material material) { await _context.Materiales.AddAsync(material); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Material material) { _context.Materiales.Update(material); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.Materiales.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
