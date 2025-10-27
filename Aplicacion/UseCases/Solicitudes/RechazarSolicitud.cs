using Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Aplication.UseCases.Solicitudes
{
	public class RechazarSolicitud
	{
		private readonly ISolicitudRepositorio _solicitudes;

		public RechazarSolicitud(ISolicitudRepositorio solicitudes)
		{
			_solicitudes = solicitudes;
		}

		public async Task EjecutarAsync(int solicitudId, string motivo = "Rechazada por el encargado")
		{
			var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId)
				?? throw new ArgumentException($"Solicitud {solicitudId} no encontrada.");

			if (!string.Equals(solicitud.EstadoSolicitud, "Pendiente", System.StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException("Esta solicitud ya fue procesada.");

			solicitud.EstadoSolicitud = motivo;
			await _solicitudes.ActualizarAsync(solicitud);
		}
	}
}