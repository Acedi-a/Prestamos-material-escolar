using System;
using System.Collections.Generic;

namespace Aplication.DTOs
{
 public class PrestamoDTO
 {
 public int Id { get; set; }
 public int SolicitudId { get; set; }
 public DateTime FechaPrestamo { get; set; }
 public DateTime FechaDevolucionPrevista { get; set; }
 public string EstadoPrestamo { get; set; } = string.Empty;
 public List<PrestamoDetalleDTO> Detalles { get; set; } = new();
 }
}
