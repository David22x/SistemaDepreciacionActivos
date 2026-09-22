using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.UseCases.IniciarSesion;

public class IniciarSesionHandler
{
    private readonly IUsuarioRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public IniciarSesionHandler(
        IUsuarioRepository repo,
        IPasswordHasher hasher,
        IJwtTokenGenerator jwt)
    {
        _repo = repo;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<string?> HandleAsync(IniciarSesionCommand cmd)
    {
        var usuario = await _repo.ObtenerPorNombreUsuarioAsync(cmd.NombreUsuario);
        if (usuario is null) return null;

        if (!_hasher.Verificar(cmd.Password, usuario.PasswordHash))
            return null;

        return _jwt.GenerarToken(usuario);
    }
}