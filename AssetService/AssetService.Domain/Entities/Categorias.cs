namespace AssetService.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int VidaUtilMeses { get; set; }

    // Navegación
    public ICollection<Activo> Activos { get; set; } = new List<Activo>();
}