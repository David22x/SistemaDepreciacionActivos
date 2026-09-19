using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Services;

public class AuthService
{
    private readonly AuthDbContext _context;
    private readonly JwtService _jwt;

    public AuthService(AuthDbContext context, JwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    public async Task<string?> LoginAsync(string nombreUsuario, string password)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        if (usuario is null) return null;

        // ⚠️ En producción usa BCrypt o ASP.NET Identity para hashear
        if (usuario.PasswordHash != password) return null;

        return _jwt.GenerarToken(usuario);
    }

    public async Task<Usuario> RegistrarAsync(string nombreUsuario,
        string email, string password)
    {
        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario,
            Email = email,
            PasswordHash = password, // ⚠️ Hashear en producción
            Rol = "Usuario"
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}