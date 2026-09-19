using AssetService.API.DTOs;
using AssetService.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Toda la clase requiere JWT
public class CategoriasController : ControllerBase
{
    private readonly AssetDbContext _context;

    public CategoriasController(AssetDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategorias()
    {
        var categorias = await _context.Categorias
            .Select(c => new CategoriaResponse(c.Id, c.Nombre, c.VidaUtilMeses))
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaResponse>> GetCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();

        return Ok(new CategoriaResponse(categoria.Id, categoria.Nombre, categoria.VidaUtilMeses));
    }
}