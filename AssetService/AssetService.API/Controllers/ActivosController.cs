using AssetService.API.DTOs;
using AssetService.Application.Common.Interfaces;
using AssetService.Application.UseCases.Activos.CrearActivo;
using AssetService.Application.UseCases.Activos.ObtenerActivos;
using AssetService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivosController : ControllerBase
{
    private readonly IActivoRepository _activos;
    private readonly ICategoriaRepository _categorias;
    private readonly CrearActivoHandler _crear;
    private readonly ObtenerActivosHandler _obtenerTodos;

    public ActivosController(
        IActivoRepository activos,
        ICategoriaRepository categorias,
        CrearActivoHandler crear,
        ObtenerActivosHandler obtenerTodos)
    {
        _activos = activos;
        _categorias = categorias;
        _crear = crear;
        _obtenerTodos = obtenerTodos;
    }

    // GET: api/activos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivoResponse>>> GetActivos()
    {
        var activos = await _obtenerTodos.HandleAsync();

        return Ok(activos.Select(MapToResponse));
    }

    // GET: api/activos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivoResponse>> GetActivo(int id)
    {
        var activo = await _activos.ObtenerPorIdAsync(id);

        if (activo is null) return NotFound(new { mensaje = "Activo no encontrado" });

        return Ok(MapToResponse(activo));
    }

    // POST: api/activos
    [HttpPost]
    public async Task<ActionResult<ActivoResponse>> CrearActivo([FromBody] ActivoCreateRequest request)
    {
        try
        {
            var activo = await _crear.HandleAsync(new CrearActivoCommand(
                request.Nombre, request.ValorOriginal,
                request.FechaAdquisicion, request.CategoriaId));

            activo = await _activos.ObtenerPorIdAsync(activo.Id) ?? activo;

            return CreatedAtAction(nameof(GetActivo), new { id = activo.Id }, MapToResponse(activo));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // PUT: api/activos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditarActivo(int id, [FromBody] ActivoUpdateRequest request)
    {
        var activo = await _activos.ObtenerPorIdAsync(id);
        if (activo is null) return NotFound(new { mensaje = "Activo no encontrado" });

        if (request.ValorOriginal <= 0)
            return BadRequest(new { mensaje = "El valor original debe ser mayor a 0" });

        var categoria = await _categorias.ObtenerPorIdAsync(request.CategoriaId);
        if (categoria is null)
            return BadRequest(new { mensaje = "La categoría no existe" });

        activo.Nombre = request.Nombre;
        activo.ValorOriginal = request.ValorOriginal;
        activo.FechaAdquisicion = request.FechaAdquisicion;
        activo.CategoriaId = request.CategoriaId;

        await _activos.ActualizarAsync(activo);
        return NoContent();
    }

    // DELETE: api/activos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarActivo(int id)
    {
        if (!await _activos.ExisteAsync(id))
            return NotFound(new { mensaje = "Activo no encontrado" });

        await _activos.EliminarAsync(id);
        return NoContent();
    }

    // Helper de mapeo
    private static ActivoResponse MapToResponse(Activo a) =>
        new(a.Id, a.Nombre, a.ValorOriginal, a.FechaAdquisicion,
            a.CategoriaId, a.Categoria?.Nombre ?? "", a.Categoria?.VidaUtilMeses ?? 0);
}

