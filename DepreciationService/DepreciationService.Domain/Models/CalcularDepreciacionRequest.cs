namespace DepreciationService.Domain.Models;

public class CalcularDepreciacionRequest
{
    public int ActivoId { get; set; }
    public DateTime? FechaConsulta { get; set; } // null = hoy
    public DateTime? FechaAdquisicion { get; set; } // opcional, permite sobreescribir la fecha del activo
}