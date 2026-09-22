using AssetService.Application.Common.Interfaces;
using AssetService.Domain.Entities;
using AssetService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AssetDbContext _context;
    public CategoriaRepository(AssetDbContext context) => _context = context;

    public Task<List<Categoria>> ObtenerTodasAsync() =>
        _context.Categorias.ToListAsync();

    public Task<Categoria?> ObtenerPorIdAsync(int id) =>
        _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
}