using System;

namespace Aplication.DTOs
{
 public class HistorialReparacionDTO
 {
 public int Id { get; set; }
 public int MaterialId { get; set; }
 public DateTime FechaEnvio { get; set; }
 public DateTime? FechaRetorno { get; set; }
 public string DescripcionFalla { get; set; } = string.Empty;
 public decimal? Costo { get; set; }
 public int Cantidad { get; set; }
 }
}
