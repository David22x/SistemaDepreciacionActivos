using AssetService.Application.Common.Interfaces;
using AssetService.Domain.Entities;

namespace AssetService.Application.UseCases.Activos.CrearActivo;

public class CrearActivoHandler
{
    private readonly IActivoRepository _activos;
    private readonly ICategoriaRepository _categorias;

    public CrearActivoHandler(IActivoRepository activos, ICategoriaRepository categorias)
    {
        _activos = activos;
        _categorias = categorias;
    }

    public async Task<Activo> HandleAsync(CrearActivoCommand cmd)
    {
        if (string.IsNullOrWhiteSpace(cmd.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (cmd.ValorOriginal <= 0)
            throw new ArgumentException("El valor original debe ser mayor a cero.");

        var categoria = await _categorias.ObtenerPorIdAsync(cmd.CategoriaId)
            ?? throw new ArgumentException("La categoría no existe.");

        var activo = new Activo
        {
            Nombre = cmd.Nombre.Trim(),
            ValorOriginal = cmd.ValorOriginal,
            FechaAdquisicion = cmd.FechaAdquisicion,
            CategoriaId = categoria.Id,
            UsuarioId = cmd.UsuarioId
        };

        return await _activos.CrearAsync(activo);
    }
}