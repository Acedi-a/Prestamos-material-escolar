using Aplication.Mapping;
using Aplication.UseCases;
using Aplication.UseCases.Devoluciones;
using Aplication.UseCases.Docentes;
using Aplication.UseCases.Materiales;
using Aplication.UseCases.Movimientos;
using Aplication.UseCases.Prestamos;
using Aplication.UseCases.Reparaciones;
using Aplication.UseCases.Reportes;
using Aplication.UseCases.Solicitudes;
using Dominio.Interfaces;
using Infraestructura.Data;
using Infraestructura.Repositorios;
using Infraestructura.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            // -----------------------------------------------------------------
            // 1. AGREGA TODOS TUS SERVICIOS AQUÍ
            // -----------------------------------------------------------------

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Infraestructura")));

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Repositorios
            builder.Services.AddScoped<IDocenteRepositorio, DocenteRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IRolRepositorio, RolRepositorio>();
            builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            builder.Services.AddScoped<ITipoReporteRepositorio, TipoReporteRepositorio>();
            builder.Services.AddScoped<IMaterialRepositorio, MaterialRepositorio>();
            builder.Services.AddScoped<IHistorialReparacionRepositorio, HistorialReparacionRepositorio>();
            builder.Services.AddScoped<ISolicitudRepositorio, SolicitudRepositorio>();
            builder.Services.AddScoped<ISolicitudDetalleRepositorio, SolicitudDetalleRepositorio>();
            builder.Services.AddScoped<IPrestamoRepositorio, PrestamoRepositorio>();
            builder.Services.AddScoped<IPrestamoDetalleRepositorio, PrestamoDetalleRepositorio>();
            builder.Services.AddScoped<IDevolucionRepositorio, DevolucionRepositorio>();
            builder.Services.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();
            builder.Services.AddScoped<IRegistroReporteRepositorio, RegistroReporteRepositorio>();

            // Servicios de infraestructura
            builder.Services.AddScoped<INotificacionServicio, NotificacionServicio>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Use Cases
            builder.Services.AddScoped<CrearDocente>(); // ejemplo antiguo
            builder.Services.AddScoped<RegistrarDocente>();
            builder.Services.AddScoped<RegistrarMaterial>();
            builder.Services.AddScoped<SolicitarMaterial>();
            builder.Services.AddScoped<RegistrarPrestamo>();
            builder.Services.AddScoped<RegistrarDevolucion>();
            builder.Services.AddScoped<ConsultarHistorialMovimientos>();
            builder.Services.AddScoped<GenerarReportePrestamosYDevoluciones>();
            builder.Services.AddScoped<ActualizarEstadoMaterial>();
            builder.Services.AddScoped<NotificarDocenteMaterialNoDisponible>();
            builder.Services.AddScoped<RegistrarEnvioAReparacion>();
            builder.Services.AddScoped<ConsultarDisponibilidadMaterial>();
            builder.Services.AddScoped<ConsultarHistorialReparaciones>();
            builder.Services.AddScoped<CompletarReparacion>();
            builder.Services.AddScoped<ActualizarMaterial>();


            // Auth (JWT)
            var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-secret-key-please-change-0123456789"; // >=32 chars
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // -----------------------------------------------------------------
            // 2. CONSTRUYE LA APLICACIÓN (SOLO UNA VEZ)
            // -----------------------------------------------------------------
            var app = builder.Build();

            // -----------------------------------------------------------------
            // 3. CONFIGURA EL PIPELINE DE HTTP (SOLO CÓDIGO 'app.Use...')
            // -----------------------------------------------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // -----------------------------------------------------------------
            // 4. EJECUTA LA APLICACIÓN
            // -----------------------------------------------------------------
            app.Run();
        }
    }
}