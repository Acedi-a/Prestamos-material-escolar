using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases.Docentes
{
 public class RegistrarDocente
 {
 private readonly IDocenteRepositorio _docentes;
 private readonly IUsuarioRepositorio _usuarios;

 public RegistrarDocente(IDocenteRepositorio docentes, IUsuarioRepositorio usuarios)
 {
 _docentes = docentes;
 _usuarios = usuarios;
 }

 public async Task<int> EjecutarAsync(string nombre, string apellido, string cedula, int usuarioId)
 {
 // Validaciones b�sicas
 if (string.IsNullOrWhiteSpace(nombre) || nombre.Length <2) throw new System.ArgumentException("Nombre inv�lido");
 if (string.IsNullOrWhiteSpace(apellido) || apellido.Length <2) throw new System.ArgumentException("Apellido inv�lido");
 if (string.IsNullOrWhiteSpace(cedula)) throw new System.ArgumentException("C�dula inv�lida");

 var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId) ?? throw new System.ArgumentException("Usuario no existe");

 var docente = new Docente
 {
 Nombre = nombre,
 Apellido = apellido,
 CedulaIdentidad = cedula,
 UsuarioId = usuario.Id
 };
 await _docentes.CrearAsync(docente);
 return docente.Id;
 }
 }
}
