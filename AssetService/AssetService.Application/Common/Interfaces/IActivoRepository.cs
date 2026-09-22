using AssetService.Domain.Entities;

namespace AssetService.Application.Common.Interfaces;

public interface IActivoRepository
{
    Task<List<Activo>> ObtenerTodosAsync();
    Task<Activo?> ObtenerPorIdAsync(int id);
    Task<Activo> CrearAsync(Activo activo);
    Task ActualizarAsync(Activo activo);
    Task EliminarAsync(int id);
    Task<bool> ExisteAsync(int id);
}