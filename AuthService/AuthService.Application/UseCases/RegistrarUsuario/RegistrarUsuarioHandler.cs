using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Entities;

namespace AuthService.Application.UseCases.RegistrarUsuario;

public class RegistrarUsuarioHandler
{
    private readonly IUsuarioRepository _repo;
    private readonly IPasswordHasher _hasher;

    public RegistrarUsuarioHandler(IUsuarioRepository repo, IPasswordHasher hasher)
    {
        _repo = repo;
        _hasher = hasher;
    }

    public async Task<Usuario> HandleAsync(RegistrarUsuarioCommand cmd)
    {
        if (await _repo.ExisteNombreUsuarioAsync(cmd.NombreUsuario))
            throw new InvalidOperationException("El nombre de usuario ya está en uso.");

        if (await _repo.ExisteEmailAsync(cmd.Email))
            throw new InvalidOperationException("El email ya está registrado.");

        var usuario = new Usuario
        {
            NombreUsuario = cmd.NombreUsuario,
            Email = cmd.Email,
            PasswordHash = _hasher.Hash(cmd.Password),
            Rol = "Usuario"
        };

        return await _repo.CrearAsync(usuario);
    }
}
