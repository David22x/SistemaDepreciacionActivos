namespace ReportService.Domain.Models;

public class ReporteDepreciacionDto
{
    public string NombreActivo { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal ValorOriginal { get; set; }
    public decimal DescuentoMensual { get; set; }
    public decimal DescuentoAcumulado { get; set; }
    public decimal ValorActual { get; set; }
    public DateTime FechaConsulta { get; set; }
    public int MesesTranscurridos { get; set; }
}