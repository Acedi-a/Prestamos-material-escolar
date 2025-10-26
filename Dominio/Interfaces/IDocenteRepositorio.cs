using Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IDocenteRepositorio
    {
        Task<Docente?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Docente>> ListarTodosAsync();
        Task CrearAsync(Docente docente);
        Task ActualizarAsync(Docente docente);
        Task EliminarAsync(int id);
		Task<Docente?> ObtenerPorUsuarioIdAsync(int usuarioId);
	}

}
