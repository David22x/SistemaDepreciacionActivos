using AssetService.API.DTOs;
using AssetService.Domain.Entities;
using AssetService.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivosController : ControllerBase
{
    private readonly AssetDbContext _context;

    public ActivosController(AssetDbContext context) => _context = context;

    // GET: api/activos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivoResponse>>> GetActivos()
    {
        var activos = await _context.Activos
            .Include(a => a.Categoria)
            .Select(a => MapToResponse(a))
            .ToListAsync();

        return Ok(activos);
    }

    // GET: api/activos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivoResponse>> GetActivo(int id)
    {
        var activo = await _context.Activos
            .Include(a => a.Categoria)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activo is null) return NotFound(new { mensaje = "Activo no encontrado" });

        return Ok(MapToResponse(activo));
    }

    // POST: api/activos
    [HttpPost]
    public async Task<ActionResult<ActivoResponse>> CrearActivo([FromBody] ActivoCreateRequest request)
    {
        if (request.ValorOriginal <= 0)
            return BadRequest(new { mensaje = "El valor original debe ser mayor a 0" });

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.Id == request.CategoriaId);

        if (!categoriaExiste)
            return BadRequest(new { mensaje = "La categoría no existe" });

        var activo = new Activo
        {
            Nombre = request.Nombre,
            ValorOriginal = request.ValorOriginal,
            FechaAdquisicion = request.FechaAdquisicion,
            CategoriaId = request.CategoriaId
        };

        _context.Activos.Add(activo);
        await _context.SaveChangesAsync();

        await _context.Entry(activo).Reference(a => a.Categoria).LoadAsync();

        return CreatedAtAction(nameof(GetActivo), new { id = activo.Id }, MapToResponse(activo));
    }

    // PUT: api/activos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditarActivo(int id, [FromBody] ActivoUpdateRequest request)
    {
        var activo = await _context.Activos.FindAsync(id);
        if (activo is null) return NotFound(new { mensaje = "Activo no encontrado" });

        if (request.ValorOriginal <= 0)
            return BadRequest(new { mensaje = "El valor original debe ser mayor a 0" });

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.Id == request.CategoriaId);
        if (!categoriaExiste)
            return BadRequest(new { mensaje = "La categoría no existe" });

        activo.Nombre = request.Nombre;
        activo.ValorOriginal = request.ValorOriginal;
        activo.FechaAdquisicion = request.FechaAdquisicion;
        activo.CategoriaId = request.CategoriaId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/activos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarActivo(int id)
    {
        var activo = await _context.Activos.FindAsync(id);
        if (activo is null) return NotFound(new { mensaje = "Activo no encontrado" });

        _context.Activos.Remove(activo);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Helper de mapeo
    private static ActivoResponse MapToResponse(Activo a) =>
        new(a.Id, a.Nombre, a.ValorOriginal, a.FechaAdquisicion,
            a.CategoriaId, a.Categoria?.Nombre ?? "", a.Categoria?.VidaUtilMeses ?? 0);
}