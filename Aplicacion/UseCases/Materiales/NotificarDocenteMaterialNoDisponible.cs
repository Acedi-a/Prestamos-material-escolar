using System.Linq;
using System.Threading.Tasks;
using Dominio.Interfaces;
using Dominio.Entities; // Asegúrate de tener este using
using System; // Para Console.WriteLine

namespace Aplication.UseCases.Materiales
{
	public class NotificarDocenteMaterialNoDisponible
	{
		private readonly ISolicitudRepositorio _solicitudes;
		private readonly ISolicitudDetalleRepositorio _detalles;
		private readonly IMaterialRepositorio _materiales;
		private readonly INotificacionServicio _notifier;
		private readonly IDocenteRepositorio _docenteRepo;   // <-- AÑADIDO
		private readonly IUsuarioRepositorio _usuarioRepo;   // <-- AÑADIDO

		public NotificarDocenteMaterialNoDisponible(
			ISolicitudRepositorio solicitudes,
			ISolicitudDetalleRepositorio detalles,
			IMaterialRepositorio materiales,
			INotificacionServicio notifier,
			IDocenteRepositorio docenteRepo,   // <-- AÑADIDO
			IUsuarioRepositorio usuarioRepo    // <-- AÑADIDO
			)
		{
			_solicitudes = solicitudes;
			_detalles = detalles;
			_materiales = materiales;
			_notifier = notifier;
			_docenteRepo = docenteRepo;       // <-- AÑADIDO
			_usuarioRepo = usuarioRepo;       // <-- AÑADIDO
		}

		public async Task EjecutarAsync(int solicitudId)
		{
			var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId)
				?? throw new System.ArgumentException("Solicitud no existe");

			// Buscar al docente UNA VEZ fuera del bucle para eficiencia
			var docenteSolicitante = await _docenteRepo.ObtenerPorIdAsync(solicitud.DocenteId);
			if (docenteSolicitante == null)
			{
				Console.WriteLine($"WARN: No se encontró docente {solicitud.DocenteId} para notificar sobre solicitud {solicitudId}.");
				return; // Salir si no encontramos al docente
			}

			// Buscar el usuario asociado al docente UNA VEZ
			var usuarioDelDocente = await _usuarioRepo.ObtenerPorIdAsync(docenteSolicitante.UsuarioId);
			if (string.IsNullOrWhiteSpace(usuarioDelDocente?.Email))
			{
				Console.WriteLine($"WARN: No se encontró email para notificar al docente {solicitud.DocenteId} (UsuarioId: {docenteSolicitante.UsuarioId}) para solicitud {solicitudId}.");
				return; // Salir si no hay email
			}
			// --- FIN Búsqueda de Email ---

			var detalles = (await _detalles.ListarTodosAsync()).Where(d => d.SolicitudId == solicitud.Id).ToList();

			foreach (var det in detalles)
			{
				var material = await _materiales.ObtenerPorIdAsync(det.MaterialId);
				if (material == null) continue;

				// Condición para notificar (stock insuficiente O estado no disponible)
				if (material.Estado != "Disponible" || material.CantidadDisponible < det.CantidadSolicitada)
				{
					var asunto = $"Material no disponible: {material.NombreMaterial} (Solicitud #{solicitudId})";
					var mensaje = $"El material '{material.NombreMaterial}' que solicitaste (Solicitud #{solicitudId}) " +
								  $"no está disponible (Estado: {material.Estado}) o el stock es insuficiente " +
								  $"(Solicitados: {det.CantidadSolicitada}, Disponibles: {material.CantidadDisponible}).";

					// Usa el email encontrado previamente
					await _notifier.EnviarPorEmailAsync(usuarioDelDocente.Email, asunto, mensaje); // <-- CORREGIDO: Pasar email (string)

					// Opcional: ¿Quieres notificar solo una vez por solicitud o por cada material problemático?
					// Si es solo una vez, puedes poner 'break;' aquí.
				}
			}
		}
	}
}