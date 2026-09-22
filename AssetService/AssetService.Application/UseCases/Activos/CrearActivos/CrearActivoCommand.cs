namespace AssetService.Application.UseCases.Activos.CrearActivo;

public record CrearActivoCommand(
    string Nombre,
    decimal ValorOriginal,
    DateTime FechaAdquisicion,
    int CategoriaId);