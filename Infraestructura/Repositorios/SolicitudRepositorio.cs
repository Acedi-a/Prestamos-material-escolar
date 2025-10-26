using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class SolicitudRepositorio : ISolicitudRepositorio
 {
 private readonly AppDbContext _context;
 public SolicitudRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<Solicitud?> ObtenerPorIdAsync(int id) => await _context.Solicitudes.FindAsync(id);
 public async Task<IEnumerable<Solicitud>> ListarTodosAsync() => await _context.Solicitudes.ToListAsync();
 public async Task CrearAsync(Solicitud solicitud) { await _context.Solicitudes.AddAsync(solicitud); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(Solicitud solicitud) { _context.Solicitudes.Update(solicitud); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.Solicitudes.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
