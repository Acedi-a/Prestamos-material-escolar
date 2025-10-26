using System;
using System.Collections.Generic;

namespace Aplication.DTOs
{
 public class SolicitudDTO
 {
 public int Id { get; set; }
 public int DocenteId { get; set; }
 public DateTime FechaSolicitud { get; set; }
 public string EstadoSolicitud { get; set; } = string.Empty;
 public List<SolicitudDetalleDTO> Detalles { get; set; } = new();
 }
}
