using DepreciationService.Domain.Models;

namespace DepreciationService.Application.Common.Interfaces;

public interface IAssetServiceClient
{
    Task<ActivoDto?> ObtenerActivoAsync(int activoId, string bearerToken);
}