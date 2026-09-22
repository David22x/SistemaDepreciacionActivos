using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AuthDbContext _context;

    public UsuarioRepository(AuthDbContext context) => _context = context;

    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

    public Task<Usuario?> ObtenerPorIdAsync(int id) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

    public Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario) =>
        _context.Usuarios.AnyAsync(u => u.NombreUsuario == nombreUsuario);

    public Task<bool> ExisteEmailAsync(string email) =>
        _context.Usuarios.AnyAsync(u => u.Email == email);

    public async Task<Usuario> CrearAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}