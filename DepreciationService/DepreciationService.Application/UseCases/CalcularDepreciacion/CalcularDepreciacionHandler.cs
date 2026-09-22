using DepreciationService.Application.Common.Interfaces;
using DepreciationService.Domain.Models;
using DepreciationService.Domain.Services;

namespace DepreciationService.Application.UseCases.CalcularDepreciacion;

public class CalcularDepreciacionHandler
{
    private readonly IAssetServiceClient _assetClient;
    private readonly DepreciacionCalculator _calculator;

    public CalcularDepreciacionHandler(
        IAssetServiceClient assetClient,
        DepreciacionCalculator calculator)
    {
        _assetClient = assetClient;
        _calculator = calculator;
    }

    public async Task<DepreciacionResponse?> HandleAsync(
        CalcularDepreciacionCommand cmd, string bearerToken)
    {
        var activo = await _assetClient.ObtenerActivoAsync(cmd.ActivoId, bearerToken);
        if (activo is null) return null;

        if (cmd.FechaAdquisicion.HasValue)
            activo.FechaAdquisicion = cmd.FechaAdquisicion.Value;

        return _calculator.Calcular(activo, cmd.FechaConsulta);
    }
}