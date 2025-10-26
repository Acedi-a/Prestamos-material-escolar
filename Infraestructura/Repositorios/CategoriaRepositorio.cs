using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class CategoriaRepositorio : ICategoriaRepositorio
 {
 private readonly AppDbContext _context;
 public CategoriaRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Categoria?> ObtenerPorIdAsync(int id) => await _context.Categorias.FindAsync(id);
 public async Task<IEnumerable<Categoria>> ListarTodosAsync() => await _context.Categorias.ToListAsync();
 public async Task CrearAsync(Categoria categoria) { await _context.Categorias.AddAsync(categoria); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Categoria categoria) { _context.Categorias.Update(categoria); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var categoria = await ObtenerPorIdAsync(id);
 if (categoria != null) { _context.Categorias.Remove(categoria); await _context.SaveChangesAsync(); }
 }
 }
}
