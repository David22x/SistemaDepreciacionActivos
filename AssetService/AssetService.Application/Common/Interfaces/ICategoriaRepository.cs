using AssetService.Domain.Entities;

namespace AssetService.Application.Common.Interfaces;

public interface ICategoriaRepository
{
    Task<List<Categoria>> ObtenerTodasAsync();
    Task<Categoria?> ObtenerPorIdAsync(int id);
}