using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class CategoriaController : ControllerBase
 {
 private readonly ICategoriaRepositorio _categorias;

 public CategoriaController(ICategoriaRepositorio categorias)
 {
 _categorias = categorias;
 }

 // GET: api/Categoria
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _categorias.ListarTodosAsync();
 var result = list.Select(c => new { c.Id, c.NombreCategoria, c.Descripcion });
 return Ok(result);
 }

 // GET: api/Categoria/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var cat = await _categorias.ObtenerPorIdAsync(id);
 if (cat == null) return NotFound();
 return Ok(new { cat.Id, cat.NombreCategoria, cat.Descripcion });
 }

 public record CategoriaCreateRequest(string NombreCategoria, string? Descripcion);
 public record CategoriaUpdateRequest(string NombreCategoria, string? Descripcion);

 // POST: api/Categoria
 [HttpPost]
 public async Task<IActionResult> Create([FromBody] CategoriaCreateRequest req)
 {
 if (string.IsNullOrWhiteSpace(req.NombreCategoria))
 return BadRequest(new { message = "El nombre de la categoría es requerido" });

 var entity = new Categoria { NombreCategoria = req.NombreCategoria.Trim(), Descripcion = req.Descripcion };
 await _categorias.CrearAsync(entity);
 return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new { id = entity.Id });
 }

 // PUT: api/Categoria/{id}
 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] CategoriaUpdateRequest req)
 {
 var entity = await _categorias.ObtenerPorIdAsync(id);
 if (entity == null) return NotFound();
 if (string.IsNullOrWhiteSpace(req.NombreCategoria))
 return BadRequest(new { message = "El nombre de la categoría es requerido" });
 entity.NombreCategoria = req.NombreCategoria.Trim();
 entity.Descripcion = req.Descripcion;
 await _categorias.ActualizarAsync(entity);
 return Ok(new { message = "Categoría actualizada" });
 }

 // DELETE: api/Categoria/{id}
 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 await _categorias.EliminarAsync(id);
 return Ok(new { message = "Categoría eliminada" });
 }
 }
}
