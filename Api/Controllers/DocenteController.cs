using Aplication.UseCases.Docentes;
using Microsoft.AspNetCore.Mvc;
using Dominio.Interfaces;
using Dominio.Entities; 
using System.Linq;      
using System;            
using System.Threading.Tasks;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DocenteController : ControllerBase
	{
		private readonly IDocenteRepositorio _docentes;
		private readonly IUsuarioRepositorio _usuarios; 
		private readonly RegistrarDocente _registrarDocente;

		
		public DocenteController(
			IDocenteRepositorio docentes,
			RegistrarDocente registrarDocente,
			IUsuarioRepositorio usuarios) 
		{
			_docentes = docentes;
			_registrarDocente = registrarDocente;
			_usuarios = usuarios; 
		}
		
		[HttpGet("PorUsuario/{usuarioId:int}")]
		public async Task<IActionResult> GetByUsuarioId(int usuarioId)
		{
			var docente = await _docentes.ObtenerPorUsuarioIdAsync(usuarioId);

			if (docente == null)
				return NotFound(new { message = "No se encontró un docente asociado a este usuario" });

			return Ok(new
			{
				docente.Id,
				docente.Nombre,
				docente.Apellido,
				docente.CedulaIdentidad,
				docente.UsuarioId
			});
		}

		// GET: api/Docente
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var docentes = await _docentes.ListarTodosAsync();
			var result = docentes.Select(d => new { d.Id, d.Nombre, d.Apellido, d.CedulaIdentidad, d.UsuarioId });
			return Ok(result);
		}

		// GET: api/Docente/{id}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var docente = await _docentes.ObtenerPorIdAsync(id);
			if (docente == null) return NotFound();
			return Ok(new { docente.Id, docente.Nombre, docente.Apellido, docente.CedulaIdentidad, docente.UsuarioId });
		}

		
		public record RegistrarDocenteRequest(int UsuarioId, string Nombre, string Apellido, string CedulaIdentidad);

		
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] RegistrarDocenteRequest request)
		{
			try
			{
				var id = await _registrarDocente.EjecutarAsync(request.Nombre, request.Apellido, request.CedulaIdentidad, request.UsuarioId);
				return CreatedAtAction(nameof(GetById), new { id }, new { id });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		

		
		public record ActualizarDocenteRequest(int UsuarioId, string Nombre, string Apellido, string CedulaIdentidad);

		
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, [FromBody] ActualizarDocenteRequest request)
		{
			
			var docente = await _docentes.ObtenerPorIdAsync(id);
			if (docente == null)
				return NotFound(new { message = "Docente no encontrado" });

			
			if (await _usuarios.ObtenerPorIdAsync(request.UsuarioId) is null)
				return BadRequest(new { message = "El UsuarioId proporcionado no existe" });

			
			if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Apellido) || string.IsNullOrWhiteSpace(request.CedulaIdentidad))
				return BadRequest(new { message = "Nombre, Apellido y Cédula son requeridos" });

			
			docente.Nombre = request.Nombre.Trim();
			docente.Apellido = request.Apellido.Trim();
			docente.CedulaIdentidad = request.CedulaIdentidad.Trim();
			docente.UsuarioId = request.UsuarioId;

			
			await _docentes.ActualizarAsync(docente);

			return Ok(new { message = "Docente actualizado" });
		}

		


		// DELETE: api/Docente/{id}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			
			var docente = await _docentes.ObtenerPorIdAsync(id);
			if (docente == null)
			{
				return NotFound(new { message = "Docente no encontrado" });
			}

			await _docentes.EliminarAsync(id);
			return Ok(new { message = "Docente eliminado" });
		}
	}
}