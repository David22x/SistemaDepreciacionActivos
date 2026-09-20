namespace DepreciationService.Domain.Models;

public class ActivoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal ValorOriginal { get; set; }
    public DateTime FechaAdquisicion { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public int VidaUtilMeses { get; set; }
}