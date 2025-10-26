using System.Threading.Tasks;
using Dominio.Interfaces;
namespace Infraestructura.Servicios
{
 public class NotificacionServicio : INotificacionServicio
 {
 public Task EnviarAsync(int docenteId, string asunto, string mensaje)
 {
 // Implementaci�n de ejemplo: aqu� podr�as integrar email, SMS, WhatsApp, etc.
 System.Console.WriteLine($"[Notificaci�n] DocenteId={docenteId} | {asunto} | {mensaje}");
 return Task.CompletedTask;
 }
 }
}
