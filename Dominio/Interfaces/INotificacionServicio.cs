using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface INotificacionServicio
 {
 Task EnviarAsync(int docenteId, string asunto, string mensaje);
 }
}
