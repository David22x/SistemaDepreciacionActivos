using System.Security.Claims;
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

    // Extrae el id del usuario autenticado desde el claim "sub" del JWT
    private int UsuarioIdActual()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        return int.Parse(claim!.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivoResponse>>> GetActivos()
    {
        var activos = await _obtenerTodos.HandleAsync(UsuarioIdActual());
        return Ok(activos.Select(MapToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivoResponse>> GetActivo(int id)
    {
        var activo = await _activos.ObtenerPorIdAsync(id, UsuarioIdActual());
        if (activo is null) return NotFound(new { mensaje = "Activo no encontrado" });
        return Ok(MapToResponse(activo));
    }

    [HttpPost]
    public async Task<ActionResult<ActivoResponse>> CrearActivo([FromBody] ActivoCreateRequest request)
    {
        try
        {
            var usuarioId = UsuarioIdActual();
            var activo = await _crear.HandleAsync(new CrearActivoCommand(
                request.Nombre, request.ValorOriginal,
                request.FechaAdquisicion, request.CategoriaId, usuarioId));

            activo = await _activos.ObtenerPorIdAsync(activo.Id, usuarioId) ?? activo;

            return CreatedAtAction(nameof(GetActivo), new { id = activo.Id }, MapToResponse(activo));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditarActivo(int id, [FromBody] ActivoUpdateRequest request)
    {
        var usuarioId = UsuarioIdActual();
        var activo = await _activos.ObtenerPorIdAsync(id, usuarioId);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarActivo(int id)
    {
        var usuarioId = UsuarioIdActual();
        if (!await _activos.ExisteAsync(id, usuarioId))
            return NotFound(new { mensaje = "Activo no encontrado" });

        await _activos.EliminarAsync(id);
        return NoContent();
    }

    private static ActivoResponse MapToResponse(Activo a) =>
        new(a.Id, a.Nombre, a.ValorOriginal, a.FechaAdquisicion,
            a.CategoriaId, a.Categoria?.Nombre ?? "", a.Categoria?.VidaUtilMeses ?? 0);
}