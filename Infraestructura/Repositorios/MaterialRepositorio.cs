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

        public async Task<(int Prestados, int EnReparacion)> ContarPrestadosYEnReparacionPorMaterialAsync(int materialId)
        {
            // Prestados: suma CantidadPrestada de PrestamosDetalle solo si el préstamo está Activo
            var prestados = await _context.PrestamosDetalle
                .Include(pd => pd.Prestamo) // 👈 para acceder a EstadoPrestamo
                .Where(pd => pd.MaterialId == materialId && pd.Prestamo.EstadoPrestamo == "Activo")
                .SumAsync(pd => (int?)pd.CantidadPrestada) ?? 0;

            // En reparación: suma Cantidad en HistorialReparaciones donde FechaRetorno sea null (aún en reparación)
            var enReparacion = await _context.HistorialReparaciones
                .Where(h => h.MaterialId == materialId && h.FechaRetorno == null)
                .SumAsync(h => (int?)h.Cantidad) ?? 0;

            return (prestados, enReparacion);
        }

        
    }
}
