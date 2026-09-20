namespace DepreciationService.Domain.Models;

public class DepreciacionResponse
{
    public string Activo { get; set; } = string.Empty;
    public decimal ValorOriginal { get; set; }
    public decimal DescuentoPorDevaluo { get; set; }   // depreciación mensual
    public decimal DescuentoAcumulado { get; set; }    // devaluo × meses
    public decimal ValorActual { get; set; }
    public int MesesTranscurridos { get; set; }
    public DateTime FechaConsulta { get; set; }
}