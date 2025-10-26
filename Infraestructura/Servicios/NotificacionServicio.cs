using System.Threading.Tasks;
using Dominio.Interfaces;
namespace Infraestructura.Servicios
{
 public class NotificacionServicio : INotificacionServicio
 {
 public Task EnviarAsync(int docenteId, string asunto, string mensaje)
 {
 // Implementación de ejemplo: aquí podrías integrar email, SMS, WhatsApp, etc.
 System.Console.WriteLine($"[Notificación] DocenteId={docenteId} | {asunto} | {mensaje}");
 return Task.CompletedTask;
 }
 }
}
