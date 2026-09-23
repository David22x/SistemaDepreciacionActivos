namespace DepreciationService.Domain.Models;

public class DepreciacionAnualDto
{
    public int Anio { get; set; }
    public decimal ValorInicioAnio { get; set; }
    public decimal DescuentoDelAnio { get; set; }
    public decimal DescuentoAcumulado { get; set; }
    public decimal ValorFinAnio { get; set; }
}

public class DepreciacionResponse
{
    public string Activo { get; set; } = string.Empty;
    public string CategoriaNombre { get; set; } = string.Empty;
    public decimal ValorOriginal { get; set; }
    public decimal DescuentoPorDevaluo { get; set; }
    public decimal DescuentoAcumulado { get; set; }
    public decimal ValorActual { get; set; }
    public int MesesTranscurridos { get; set; }
    public DateTime FechaAdquisicion { get; set; }
    public DateTime FechaConsulta { get; set; }

    /// <summary>Una fila por cada año calendario entre la adquisición y la fecha de consulta.</summary>
    public List<DepreciacionAnualDto> DesglosePorAnio { get; set; } = new();
}