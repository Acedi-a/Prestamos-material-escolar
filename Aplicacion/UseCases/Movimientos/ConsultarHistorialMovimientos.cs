using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplication.DTOs;
using Dominio.Interfaces;

namespace Aplication.UseCases.Movimientos
{
    public class ConsultarHistorialMovimientos
    {
        private readonly IMovimientoRepositorio _movimientos;
        public ConsultarHistorialMovimientos(IMovimientoRepositorio movimientos) { _movimientos = movimientos; }

        public async Task<IEnumerable<MovimientoDTO>> EjecutarAsync(int? materialId = null)
        {
            var lista = await _movimientos.ListarTodosAsync();
            if (materialId.HasValue) lista = lista.Where(m => m.MaterialId == materialId.Value);

            return lista.Select(m => new MovimientoDTO
            {
                Id = m.Id,
                MaterialId = m.MaterialId,
                MaterialNombre = m.Material != null ? m.Material.NombreMaterial : string.Empty,
                TipoMovimiento = m.TipoMovimiento,
                FechaMovimiento = m.FechaMovimiento,
                Cantidad = m.Cantidad,
                PrestamoId = m.PrestamoId
            });
        }
    }
}
