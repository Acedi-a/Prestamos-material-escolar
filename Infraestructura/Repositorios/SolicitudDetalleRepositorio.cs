using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
 public class SolicitudDetalleRepositorio : ISolicitudDetalleRepositorio
 {
 private readonly AppDbContext _context;
 public SolicitudDetalleRepositorio(AppDbContext context) { _context = context; }
 
 public async Task<SolicitudDetalle?> ObtenerPorIdAsync(int id) => await _context.SolicitudesDetalle.FindAsync(id);
 public async Task<IEnumerable<SolicitudDetalle>> ListarTodosAsync() => await _context.SolicitudesDetalle.ToListAsync();
 public async Task CrearAsync(SolicitudDetalle detalle) { await _context.SolicitudesDetalle.AddAsync(detalle); await _context.SaveChangesAsync(); }
 public async Task ActualizarAsync(SolicitudDetalle detalle) { _context.SolicitudesDetalle.Update(detalle); await _context.SaveChangesAsync(); }
 public async Task EliminarAsync(int id)
 {
 var entity = await ObtenerPorIdAsync(id);
 if (entity != null) { _context.SolicitudesDetalle.Remove(entity); await _context.SaveChangesAsync(); }
 }
 }
}
