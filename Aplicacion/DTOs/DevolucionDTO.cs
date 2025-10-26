using System;

namespace Aplication.DTOs
{
 public class DevolucionDTO
 {
 public int Id { get; set; }
 public int PrestamoId { get; set; }
 public DateTime FechaDevolucion { get; set; }
 public string? Observaciones { get; set; }
 }
}
