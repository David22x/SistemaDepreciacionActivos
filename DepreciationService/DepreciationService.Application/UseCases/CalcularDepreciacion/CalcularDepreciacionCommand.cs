namespace DepreciationService.Application.UseCases.CalcularDepreciacion;

public record CalcularDepreciacionCommand(
    int ActivoId,
    DateTime FechaConsulta,
    DateTime? FechaAdquisicion = null);