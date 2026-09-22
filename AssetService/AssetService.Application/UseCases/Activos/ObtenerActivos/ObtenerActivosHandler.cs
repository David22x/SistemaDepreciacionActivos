using AssetService.Application.Common.Interfaces;
using AssetService.Domain.Entities;

namespace AssetService.Application.UseCases.Activos.ObtenerActivos;

public class ObtenerActivosHandler
{
    private readonly IActivoRepository _repo;
    public ObtenerActivosHandler(IActivoRepository repo) => _repo = repo;

    public Task<List<Activo>> HandleAsync() => _repo.ObtenerTodosAsync();
}