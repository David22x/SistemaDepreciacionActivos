using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);
    Task<bool> ExisteEmailAsync(string email);
    Task<Usuario> CrearAsync(Usuario usuario);
}