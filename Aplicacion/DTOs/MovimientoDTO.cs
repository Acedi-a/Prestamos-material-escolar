using System;

namespace Aplication.DTOs
{
     public class MovimientoDTO
     {
         public int Id { get; set; }
         public int MaterialId { get; set; }
         public string TipoMovimiento { get; set; } = string.Empty;
         public DateTime FechaMovimiento { get; set; }
         public int Cantidad { get; set; }
         public int? PrestamoId { get; set; }
        public string? MaterialNombre { get; set; } = string.Empty;

    }
}
