using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.UseCases.Solicitudes // ⬅️ Lo ponemos en la carpeta 'Solicitudes'
{
	public class AprobarSolicitud
	{
		// Todos los repositorios de tu RegistrarPrestamo
		private readonly IPrestamoRepositorio _prestamos;
		private readonly IPrestamoDetalleRepositorio _detalles;
		private readonly ISolicitudRepositorio _solicitudes;
		private readonly ISolicitudDetalleRepositorio _solSolicitudDet;
		private readonly IMaterialRepositorio _materiales;
		private readonly IMovimientoRepositorio _movimientos;

		public AprobarSolicitud(
			IPrestamoRepositorio prestamos,
			IPrestamoDetalleRepositorio detalles,
			ISolicitudRepositorio solicitudes,
			ISolicitudDetalleRepositorio solDetalles,
			IMaterialRepositorio materiales,
			IMovimientoRepositorio movimientos)
		{
			_prestamos = prestamos;
			_detalles = detalles;
			_solicitudes = solicitudes;
			_solSolicitudDet = solDetalles;
			_materiales = materiales;
			_movimientos = movimientos;
		}

		// Ya no necesita 'fechaPrevista', la calculamos
		public async Task<int> EjecutarAsync(int solicitudId, DateTime fechaPrevista)
		{
			var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId)
				?? throw new System.ArgumentException("Solicitud no existe");

			// --- CAMBIO 1 ---
			// Buscamos "Pendiente", no "Aprobada"
			if (!string.Equals(solicitud.EstadoSolicitud, "Pendiente", System.StringComparison.OrdinalIgnoreCase))
				throw new System.InvalidOperationException("La solicitud ya fue procesada (no está 'Pendiente')");

			// Basado en tu entidad Prestamo.cs, no incluimos DocenteId
			var prestamo = new Prestamo
			{
				SolicitudId = solicitud.Id,
				FechaPrestamo = System.DateTime.UtcNow,
				FechaDevolucionPrevista = fechaPrevista, // ⬅️ Usa el parámetro aquí
				EstadoPrestamo = "Activo"
			};
			await _prestamos.CrearAsync(prestamo);

			// Cargar detalles de la solicitud (tu código)
			var detallesSolicitud = (await _solSolicitudDet.ListarTodosAsync())
				.Where(d => d.SolicitudId == solicitud.Id).ToList();

			if (detallesSolicitud.Count == 0)
				throw new System.InvalidOperationException("La solicitud no tiene detalles");

			// Tu lógica de stock y movimientos (¡perfecta!)
			foreach (var sd in detallesSolicitud)
			{
				var material = await _materiales.ObtenerPorIdAsync(sd.MaterialId)
					?? throw new System.ArgumentException($"Material {sd.MaterialId} no existe");

				if (material.CantidadDisponible < sd.CantidadSolicitada)
				{
					// Si no hay stock, rechazamos automáticamente
					solicitud.EstadoSolicitud = "Rechazada (Stock Insuficiente)";
					await _solicitudes.ActualizarAsync(solicitud);
					throw new System.InvalidOperationException($"Material {material.NombreMaterial} sin stock suficiente. Solicitud rechazada.");
				}

				material.CantidadDisponible -= sd.CantidadSolicitada;
				await _materiales.ActualizarAsync(material);

				var detalle = new PrestamoDetalle { PrestamoId = prestamo.Id, MaterialId = sd.MaterialId, CantidadPrestada = sd.CantidadSolicitada };
				await _detalles.CrearAsync(detalle);

				await _movimientos.CrearAsync(new Movimiento { MaterialId = sd.MaterialId, TipoMovimiento = "Salida por Préstamo", FechaMovimiento = System.DateTime.UtcNow, Cantidad = sd.CantidadSolicitada, PrestamoId = prestamo.Id });
			}

			// --- CAMBIO 3 ---
			// Actualizamos el estado de la Solicitud
			solicitud.EstadoSolicitud = "Aprobada";
			await _solicitudes.ActualizarAsync(solicitud);

			return prestamo.Id;
		}
	}
}