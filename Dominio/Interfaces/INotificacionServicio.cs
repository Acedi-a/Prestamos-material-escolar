using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface INotificacionServicio
 {
		Task EnviarPorEmailAsync(string emailDestino, string asunto, string mensaje);
	}
}
