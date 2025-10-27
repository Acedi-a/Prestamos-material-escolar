using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Reportes
{
 public class GenerarReportePrestamosYDevoluciones
 {
 private readonly IPrestamoRepositorio _prestamos;
 private readonly IDevolucionRepositorio _devoluciones;
 private readonly IRegistroReporteRepositorio _registros;
 private readonly ITipoReporteRepositorio _tipos;
 private readonly IUsuarioRepositorio _usuarios;

 public GenerarReportePrestamosYDevoluciones(IPrestamoRepositorio prestamos, IDevolucionRepositorio devoluciones, IRegistroReporteRepositorio registros, ITipoReporteRepositorio tipos, IUsuarioRepositorio usuarios)
 {
 _prestamos = prestamos; _devoluciones = devoluciones; _registros = registros; _tipos = tipos; _usuarios = usuarios;
 }

 public async Task<(IEnumerable<Prestamo> prestamos, IEnumerable<Devolucion> devoluciones)> EjecutarAsync(int usuarioId, DateTime desde, DateTime hasta)
 {
 // Validar usuario para evitar violación de FK
 _ = await _usuarios.ObtenerPorIdAsync(usuarioId) ?? throw new ArgumentException("Usuario no existe");

 var prest = (await _prestamos.ListarTodosAsync()).Where(p => p.FechaPrestamo >= desde && p.FechaPrestamo <= hasta).ToList();
 var devol = (await _devoluciones.ListarTodosAsync()).Where(d => d.FechaDevolucion >= desde && d.FechaDevolucion <= hasta).ToList();

 var tipos = await _tipos.ListarTodosAsync();
 var tipo = tipos.FirstOrDefault(t =>
 string.Equals(t.NombreReporte, "Prestamos y Devoluciones", StringComparison.OrdinalIgnoreCase) ||
 string.Equals(t.NombreReporte, "PrestamosYDevoluciones", StringComparison.OrdinalIgnoreCase) ||
 (t.NombreReporte.Contains("prestamo", StringComparison.OrdinalIgnoreCase) && t.NombreReporte.Contains("devolu", StringComparison.OrdinalIgnoreCase))
 );
 if (tipo == null)
 {
 tipo = new TipoReporte { NombreReporte = "Prestamos y Devoluciones" };
 await _tipos.CrearAsync(tipo);
 }

 await _registros.CrearAsync(new RegistroReporte
 {
 TipoReporteId = tipo.Id,
 UsuarioId = usuarioId,
 FechaGeneracion = DateTime.UtcNow,
 Parametros = $"{{\"desde\":\"{desde:o}\",\"hasta\":\"{hasta:o}\"}}"
 });
 return (prest, devol);
 }
 }
}
