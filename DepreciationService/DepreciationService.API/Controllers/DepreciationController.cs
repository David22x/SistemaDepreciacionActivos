using DepreciationService.Domain.Models;
using DepreciationService.Domain.Services;
using DepreciationService.Infrastructure.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepreciationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepreciationController : ControllerBase
{
    private readonly AssetServiceClient _assetClient;
    private readonly DepreciacionCalculator _calculator;

    public DepreciationController(
        AssetServiceClient assetClient,
        DepreciacionCalculator calculator)
    {
        _assetClient = assetClient;
        _calculator = calculator;
    }

    [HttpPost("calcular")]
    public async Task<IActionResult> Calcular(
        [FromBody] CalcularDepreciacionRequest request)
    {
        // 1. Extraer el token del header para reenviarlo a AssetService
        var token = Request.Headers["Authorization"]
            .FirstOrDefault()?.Replace("Bearer ", "");

        // 2. Obtener el activo desde AssetService
        var activo = await _assetClient.ObtenerActivoAsync(request.ActivoId, token);
        if (activo is null)
            return NotFound(new { mensaje = $"Activo {request.ActivoId} no encontrado" });

        // 3. Calcular depreciación
        var fechaConsulta = request.FechaConsulta ?? DateTime.UtcNow;
        var resultado = _calculator.Calcular(activo, fechaConsulta);

        return Ok(resultado);
    }
}