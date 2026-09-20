namespace DepreciationService.Domain.Models;

public class CalcularDepreciacionRequest
{
    public int ActivoId { get; set; }
    public DateTime? FechaConsulta { get; set; } // null = hoy
}