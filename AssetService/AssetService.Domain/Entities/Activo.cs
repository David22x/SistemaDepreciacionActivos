namespace AssetService.Domain.Entities;

public class Activo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal ValorOriginal { get; set; }
    public DateTime FechaAdquisicion { get; set; }

    // FK
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}