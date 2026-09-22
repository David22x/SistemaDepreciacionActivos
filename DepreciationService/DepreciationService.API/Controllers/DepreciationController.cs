using DepreciationService.Application.UseCases.CalcularDepreciacion;
using DepreciationService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepreciationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepreciationController : ControllerBase
{
    private readonly CalcularDepreciacionHandler _handler;

    public DepreciationController(CalcularDepreciacionHandler handler) =>
        _handler = handler;

    [HttpPost("calcular")]
    public async Task<IActionResult> Calcular([FromBody] CalcularDepreciacionRequest request)
    {
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        var fechaConsulta = request.FechaConsulta ?? DateTime.UtcNow;
        var resultado = await _handler.HandleAsync(
            new CalcularDepreciacionCommand(
                request.ActivoId, fechaConsulta, request.FechaAdquisicion), token);

        return resultado is null
            ? NotFound(new { mensaje = $"Activo {request.ActivoId} no encontrado" })
            : Ok(resultado);
    }
}