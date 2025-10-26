using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class RolRepositorio : IRolRepositorio
 {
 private readonly AppDbContext _context;
 public RolRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Rol?> ObtenerPorIdAsync(int id) => await _context.Roles.FindAsync(id);
 public async Task<IEnumerable<Rol>> ListarTodosAsync() => await _context.Roles.ToListAsync();
 public async Task CrearAsync(Rol rol) { await _context.Roles.AddAsync(rol); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Rol rol) { _context.Roles.Update(rol); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var rol = await ObtenerPorIdAsync(id);
 if (rol != null) { _context.Roles.Remove(rol); await _context.SaveChangesAsync(); }
 }
 }
}
