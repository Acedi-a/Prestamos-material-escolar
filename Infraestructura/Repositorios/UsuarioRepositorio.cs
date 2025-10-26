using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class UsuarioRepositorio : IUsuarioRepositorio
 {
 private readonly AppDbContext _context;
 public UsuarioRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Usuario?> ObtenerPorIdAsync(int id) => await _context.Usuarios.FindAsync(id);
 public async Task<IEnumerable<Usuario>> ListarTodosAsync() => await _context.Usuarios.ToListAsync();
 public async Task CrearAsync(Usuario usuario) { await _context.Usuarios.AddAsync(usuario); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Usuario usuario) { _context.Usuarios.Update(usuario); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.Usuarios.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
