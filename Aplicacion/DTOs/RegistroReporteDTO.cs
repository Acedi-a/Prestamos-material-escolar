using System;

namespace Aplication.DTOs
{
 public class RegistroReporteDTO
 {
 public int Id { get; set; }
 public int TipoReporteId { get; set; }
 public int UsuarioId { get; set; }
 public int? MovimientoId { get; set; }
 public DateTime FechaGeneracion { get; set; }
 public string? Parametros { get; set; }
 }
}
