using Aplication.UseCases.Materiales;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class MaterialController : ControllerBase
 {
 private readonly IMaterialRepositorio _materiales;
 private readonly RegistrarMaterial _registrarMaterial;
 private readonly ActualizarEstadoMaterial _actualizarEstado;
 private readonly ConsultarDisponibilidadMaterial _consultarDisponibilidad;
 private readonly ActualizarMaterial _actualizarMaterial;

 public MaterialController(IMaterialRepositorio materiales, RegistrarMaterial registrarMaterial, ActualizarEstadoMaterial actualizarEstado, ConsultarDisponibilidadMaterial consultarDisponibilidad, ActualizarMaterial actualizarMaterial)
 {
 _materiales = materiales;
 _registrarMaterial = registrarMaterial;
 _actualizarEstado = actualizarEstado;
 _consultarDisponibilidad = consultarDisponibilidad;
 _actualizarMaterial = actualizarMaterial;
 }

 // GET: api/Material
 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _materiales.ListarTodosAsync();
 return Ok(list);
 }

 // GET: api/Material/{id}
 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var m = await _materiales.ObtenerPorIdAsync(id);
 if (m == null) return NotFound();
 return Ok(new { m.Id, m.NombreMaterial, m.CategoriaId, m.CantidadTotal, m.CantidadDisponible, m.Estado, m.Descripcion });
 }

 public record RegistrarMaterialRequest(int CategoriaId, string Nombre, string? Descripcion, int CantidadInicial, string Estado);

 // POST: api/Material
 [HttpPost]
 public async Task<IActionResult> Create([FromBody] RegistrarMaterialRequest req)
 {
 try
 {
 var id = await _registrarMaterial.EjecutarAsync(req.CategoriaId, req.Nombre, req.Descripcion, req.CantidadInicial, req.Estado);
 return CreatedAtAction(nameof(GetById), new { id }, new { id });
 }
 catch (ArgumentException ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }

 public record ActualizarEstadoRequest(string Estado);

 // PUT: api/Material/{id}/estado
 [HttpPut("{id:int}/estado")]
 public async Task<IActionResult> UpdateEstado(int id, [FromBody] ActualizarEstadoRequest req)
 {
 await _actualizarEstado.EjecutarAsync(id, req.Estado);
 return Ok(new { message = "Estado actualizado" });
 }

 public record ActualizarMaterialRequest(int CategoriaId, string Nombre, string? Descripcion, int CantidadTotal, int CantidadDisponible, string Estado);

 // PUT: api/Material/{id}
 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] ActualizarMaterialRequest req)
 {
 try
 {
 await _actualizarMaterial.EjecutarAsync(id, req.CategoriaId, req.Nombre, req.Descripcion, req.CantidadTotal, req.CantidadDisponible, req.Estado);
 return Ok(new { message = "Material actualizado" });
 }
 catch (ArgumentException ex)
 {
 return BadRequest(new { message = ex.Message });
 }
 }

 // GET: api/Material/{id}/disponibilidad
 [HttpGet("{id:int}/disponibilidad")]
 public async Task<IActionResult> GetDisponibilidad(int id)
 {
 var (cantidad, estado) = await _consultarDisponibilidad.EjecutarAsync(id);
 return Ok(new { cantidadDisponible = cantidad, estado });
 }
 }
}
