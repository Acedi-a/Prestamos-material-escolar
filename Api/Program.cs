using Aplication.Mapping;
using Aplication.UseCases;
using Dominio.Interfaces;
using Infraestructura.Data;
using Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;

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



            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Infraestructura")));

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddScoped<IDocenteRepositorio, DocenteRepositorio>();
            builder.Services.AddScoped<CrearDocente>();

            

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

            app.UseAuthorization();

            app.MapControllers();

            // -----------------------------------------------------------------
            // 4. EJECUTA LA APLICACIÓN
            // -----------------------------------------------------------------
            app.Run();
        }
    }
}