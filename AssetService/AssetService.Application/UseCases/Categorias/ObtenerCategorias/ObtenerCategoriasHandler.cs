using AssetService.Application.Common.Interfaces;
using AssetService.Domain.Entities;

namespace AssetService.Application.UseCases.Categorias.ObtenerCategorias;

public class ObtenerCategoriasHandler
{
    private readonly ICategoriaRepository _repo;
    public ObtenerCategoriasHandler(ICategoriaRepository repo) => _repo = repo;

    public Task<List<Categoria>> HandleAsync() => _repo.ObtenerTodasAsync();
}