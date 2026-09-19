namespace AssetService.API.DTOs;

public record ActivoCreateRequest(
    string Nombre,
    decimal ValorOriginal,
    DateTime FechaAdquisicion,
    int CategoriaId);

public record ActivoUpdateRequest(
    string Nombre,
    decimal ValorOriginal,
    DateTime FechaAdquisicion,
    int CategoriaId);

public record ActivoResponse(
    int Id,
    string Nombre,
    decimal ValorOriginal,
    DateTime FechaAdquisicion,
    int CategoriaId,
    string CategoriaNombre,
    int VidaUtilMeses);