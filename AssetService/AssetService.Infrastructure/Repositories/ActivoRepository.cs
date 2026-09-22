using AssetService.Application.Common.Interfaces;
using AssetService.Domain.Entities;
using AssetService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Infrastructure.Repositories;

public class ActivoRepository : IActivoRepository
{
    private readonly AssetDbContext _context;
    public ActivoRepository(AssetDbContext context) => _context = context;

    public Task<List<Activo>> ObtenerTodosAsync() =>
        _context.Activos.Include(a => a.Categoria).ToListAsync();

    public Task<Activo?> ObtenerPorIdAsync(int id) =>
        _context.Activos.Include(a => a.Categoria)
                       .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Activo> CrearAsync(Activo activo)
    {
        _context.Activos.Add(activo);
        await _context.SaveChangesAsync();
        return activo;
    }

    public async Task ActualizarAsync(Activo activo)
    {
        _context.Activos.Update(activo);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var activo = await _context.Activos.FindAsync(id);
        if (activo is null) return;
        _context.Activos.Remove(activo);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExisteAsync(int id) =>
        _context.Activos.AnyAsync(a => a.Id == id);
}