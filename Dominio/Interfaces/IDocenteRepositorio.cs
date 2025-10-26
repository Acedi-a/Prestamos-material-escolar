using Dominio.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IDocenteRepositorio
    {
        Task<Docente?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Docente>> ListarTodosAsync();
        Task CrearAsync(Docente docente);
        Task ActualizarAsync(Docente docente);
        Task EliminarAsync(Guid id);
    }

}
