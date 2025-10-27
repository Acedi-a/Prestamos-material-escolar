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
		private readonly INotificacionServicio _notifier;
		private readonly IUsuarioRepositorio _usuarioRepo;
		private readonly IDocenteRepositorio _docenteRepo;

		public AprobarSolicitud(
			IPrestamoRepositorio prestamos,
			IPrestamoDetalleRepositorio detalles,
			ISolicitudRepositorio solicitudes,
			ISolicitudDetalleRepositorio solDetalles,
			IMaterialRepositorio materiales,
			IMovimientoRepositorio movimientos,INotificacionServicio notifier,     
			IUsuarioRepositorio usuarioRepo,      
			IDocenteRepositorio docenteRepo)
		{
			_prestamos = prestamos;
			_detalles = detalles;
			_solicitudes = solicitudes;
			_solSolicitudDet = solDetalles;
			_materiales = materiales;
			_movimientos = movimientos;
			_notifier = notifier;              
			_usuarioRepo = usuarioRepo;           
			_docenteRepo = docenteRepo;
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
			var docenteSolicitante = await _docenteRepo.ObtenerPorIdAsync(solicitud.DocenteId);

			
			foreach (var sd in detallesSolicitud)
			{
				var material = await _materiales.ObtenerPorIdAsync(sd.MaterialId)
					?? throw new System.ArgumentException($"Material {sd.MaterialId} no existe");


				if (material.CantidadDisponible < sd.CantidadSolicitada)
				{
					
					var asunto = $"Material no disponible: {material.NombreMaterial} (Solicitud #{solicitudId})";
					var mensaje = $"El material '{material.NombreMaterial}' que solicitaste (Solicitud #{solicitudId}) " +
								  $"no está disponible o el stock es insuficiente " +
								  $"(Solicitados: {sd.CantidadSolicitada}, Disponibles: {material.CantidadDisponible}, Estado: {material.Estado}). " +
								  "Tu solicitud ha sido rechazada por este motivo.";

					
					
					if (docenteSolicitante != null)
					{
						var usuarioDelDocente = await _usuarioRepo.ObtenerPorIdAsync(docenteSolicitante.UsuarioId);
						if (!string.IsNullOrWhiteSpace(usuarioDelDocente?.Email))
						{
							await _notifier.EnviarPorEmailAsync(usuarioDelDocente.Email, asunto, mensaje);
						}
						else
						{
							Console.WriteLine($"WARN: No se encontró email para notificar al docente {solicitud.DocenteId} (UsuarioId: {docenteSolicitante.UsuarioId})");
						}
					}
					else
					{
						Console.WriteLine($"WARN: No se encontró docente {solicitud.DocenteId} para notificar.");
					}
					// --- FIN CÓDIGO DE NOTIFICACIÓN ---

					// Código existente para rechazar y lanzar error
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
			try
			{
				var asuntoAprobado = $"✅ Solicitud Aprobada - ID #{solicitudId}";

				// Construir lista de materiales solicitados
				var listaMateriales = "";
				foreach (var sd in detallesSolicitud)
				{
					var material = await _materiales.ObtenerPorIdAsync(sd.MaterialId);
					if (material != null)
					{
						listaMateriales += $"- {material.NombreMaterial}: {sd.CantidadSolicitada} unidades\n";
					}
				}

				var mensajeAprobado = $"¡Felicitaciones! 🎉\n\n" +
									  $"Tu solicitud de material (ID: {solicitudId}) ha sido **APROBADA** exitosamente.\n\n" +
									  $"📅 **Fecha de Solicitud:** {solicitud.FechaSolicitud.ToString("dd/MM/yyyy")}\n" +
									  $"📦 **Materiales Solicitados:**\n{listaMateriales}\n" +
									  $"🔄 **Préstamo Generado:** Se ha creado un préstamo asociado con ID {prestamo.Id}.\n" +
									  $"📅 **Fecha de Devolución Prevista:** {fechaPrevista.ToString("dddd, dd 'de' MMMM 'de' yyyy 'a las' HH:mm 'hrs.'")}\n\n" +
									  $"📍 **Próximos Pasos:**\n" +
									  $"- Coordina el retiro del material con el encargado del almacén.\n" +
									  $"- Asegúrate de devolver los materiales en la fecha indicada para evitar penalizaciones.\n\n" +
									  $"Si tienes alguna duda, no dudes en contactarnos.\n\n" +
									  $"Atentamente,\n" +
									  $"Equipo de Gestión de Préstamos\n" +
									  $"Sistema de Préstamos de Material Escolar\n" +
									  $"📧 soporte@prestamos.edu.bo | 📞 +591 123-4567";

				var usuarioDelDocenteAprob = await _usuarioRepo.ObtenerPorIdAsync(docenteSolicitante.UsuarioId);
				if (!string.IsNullOrWhiteSpace(usuarioDelDocenteAprob?.Email))
				{
					await _notifier.EnviarPorEmailAsync(usuarioDelDocenteAprob.Email, asuntoAprobado, mensajeAprobado);
				}
				else
				{
					Console.WriteLine($"WARN: No se encontró email para notificar aprobación al docente {solicitud.DocenteId} (UsuarioId: {docenteSolicitante.UsuarioId})");
				}
			}
			catch (Exception exEmail)
			{
				// Loggear que la notificación falló pero no detener el proceso principal
				Console.WriteLine($"ERROR al enviar email de aprobación para Solicitud #{solicitudId}: {exEmail.Message}");
			}
			return prestamo.Id;
		}
	}
}