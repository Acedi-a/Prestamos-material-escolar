using Dominio.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infraestructura.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Docente> Docentes { get; set; }

        // Nuevas tablas según el script SQL
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoReporte> TiposReportes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<HistorialReparacion> HistorialReparaciones { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<SolicitudDetalle> SolicitudesDetalle { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<PrestamoDetalle> PrestamosDetalle { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<RegistroReporte> RegistroReportes { get; set; }

    }
}
