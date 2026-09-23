using AssetService.Domain.Entities;

namespace AssetService.Application.Common.Interfaces;

public interface IActivoRepository
{
    Task<List<Activo>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<Activo?> ObtenerPorIdAsync(int id, int usuarioId);
    Task<Activo> CrearAsync(Activo activo);
    Task ActualizarAsync(Activo activo);
    Task EliminarAsync(int id);
    Task<bool> ExisteAsync(int id, int usuarioId);
}