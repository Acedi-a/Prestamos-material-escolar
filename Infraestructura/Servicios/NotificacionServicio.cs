using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Dominio.Interfaces;
using Microsoft.Extensions.Configuration; // Para leer config
using System; // Para Console.WriteLine

namespace Infraestructura.Servicios
{
	// Cambia el nombre de la clase si el archivo se llama diferente
	public class NotificacionServicio : INotificacionServicio
	{
		private readonly IConfiguration _config;

		public NotificacionServicio(IConfiguration config)
		{
			_config = config;
		}


		// Implementa el método de la interfaz actualizada
		public async Task EnviarPorEmailAsync(string emailDestino, string asunto, string mensaje)
		{
			try
			{
				var smtpHost = _config["SmtpSettings:Host"];
				var smtpPort = int.Parse(_config["SmtpSettings:Port"] ?? "587");
				var smtpUser = _config["SmtpSettings:Username"];
				var smtpPass = _config["SmtpSettings:Password"];
				var fromEmail = _config["SmtpSettings:FromEmail"];

				if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass) || string.IsNullOrEmpty(fromEmail))
				{
					Console.WriteLine("WARN: Configuración SMTP incompleta en appsettings.json. No se envió el email.");
					return;
				}

				using (var client = new SmtpClient(smtpHost, smtpPort))
				{
					client.EnableSsl = true; // Ajusta según tu servidor
					client.Credentials = new NetworkCredential(smtpUser, smtpPass);

					var mailMessage = new MailMessage
					{
						From = new MailAddress(fromEmail),
						Subject = asunto,
						Body = mensaje,
						IsBodyHtml = false,
					};
					mailMessage.To.Add(emailDestino);

					await client.SendMailAsync(mailMessage);
					Console.WriteLine($"INFO: Email enviado a {emailDestino} con asunto: {asunto}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR: No se pudo enviar email a {emailDestino}. Error: {ex.Message}");
				// Considera relanzar la excepción si el envío es crítico
				// throw;
			}
		}
	}
}