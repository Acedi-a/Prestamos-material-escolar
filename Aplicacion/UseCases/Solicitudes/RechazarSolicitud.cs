using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace Aplication.UseCases.Solicitudes
{
	public class RechazarSolicitud
	{
		private readonly ISolicitudRepositorio _solicitudes;
		private readonly INotificacionServicio _notifier;    
		private readonly IUsuarioRepositorio _usuarioRepo;  
		private readonly IDocenteRepositorio _docenteRepo;  

		public RechazarSolicitud(
			ISolicitudRepositorio solicitudes,
			INotificacionServicio notifier,          
			IUsuarioRepositorio usuarioRepo,          
			IDocenteRepositorio docenteRepo           
			)
		{
			_solicitudes = solicitudes;
			_notifier = notifier;               
			_usuarioRepo = usuarioRepo;           
			_docenteRepo = docenteRepo;           
		}

		public async Task EjecutarAsync(int solicitudId, string motivo = "Rechazada por el encargado")
		{
			
			var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId) 
				?? throw new ArgumentException($"Solicitud {solicitudId} no encontrada.");

			
			var docenteSolicitante = await _docenteRepo.ObtenerPorIdAsync(solicitud.DocenteId);
			if (docenteSolicitante == null)
			{
				Console.WriteLine($"WARN: No se encontró docente {solicitud.DocenteId} al intentar rechazar solicitud.");
				
			}


			if (!string.Equals(solicitud.EstadoSolicitud, "Pendiente", System.StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException("Esta solicitud ya fue procesada.");

			
			solicitud.EstadoSolicitud = motivo; 
			await _solicitudes.ActualizarAsync(solicitud);

			
			try
			{
				if (docenteSolicitante != null) 
				{
					var asuntoRechazo = $"❌ Solicitud Rechazada - ID #{solicitudId}";

					var mensajeRechazo = $"Hola,\n\n" +
										 $"Lamentamos informarte que tu solicitud de material (ID: {solicitudId}) ha sido **RECHAZADA** por el encargado.\n\n" +
										 $"📅 **Fecha de Solicitud:** {solicitud.FechaSolicitud.ToString("dd/MM/yyyy")}\n" +
										 $"📝 **Motivo del Rechazo:** {motivo}\n\n" +
										 $"💡 **Próximos Pasos Recomendados:**\n" +
										 $"- Revisa los motivos y ajusta tu solicitud si es necesario.\n" +
										 $"- Contacta al encargado para más detalles o asistencia.\n" +
										 $"- Puedes enviar una nueva solicitud una vez resueltos los inconvenientes.\n\n" +
										 $"Si tienes alguna duda o necesitas ayuda, no dudes en comunicarte con nosotros.\n\n" +
										 $"Atentamente,\n" +
										 $"Equipo de Gestión de Préstamos\n" +
										 $"Sistema de Préstamos de Material Escolar\n" +
										 $"📧 soporte@prestamos.edu.bo | 📞 +591 123-4567";

					var usuarioDelDocente = await _usuarioRepo.ObtenerPorIdAsync(docenteSolicitante.UsuarioId);
					if (!string.IsNullOrWhiteSpace(usuarioDelDocente?.Email))
					{
						await _notifier.EnviarPorEmailAsync(usuarioDelDocente.Email, asuntoRechazo, mensajeRechazo);
					}
					else
					{
						Console.WriteLine($"WARN: No se encontró email para notificar rechazo al docente {solicitud.DocenteId} (UsuarioId: {docenteSolicitante.UsuarioId})");
					}
				}
			}
			catch (Exception exEmail)
			{
				Console.WriteLine($"ERROR al enviar email de rechazo para Solicitud #{solicitudId}: {exEmail.Message}");
			}
			
		}
	}
}