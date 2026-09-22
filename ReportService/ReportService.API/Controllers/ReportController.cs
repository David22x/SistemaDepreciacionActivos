using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportService.Application.UseCases.GenerarReporteDepreciacion;
using ReportService.Domain.Models;

namespace ReportService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly GenerarReporteDepreciacionHandler _handler;

    public ReportController(GenerarReporteDepreciacionHandler handler)
        => _handler = handler;

    [HttpPost("depreciacion/pdf")]
    public IActionResult GenerarPdfDepreciacion([FromBody] ReporteDepreciacionDto datos)
    {
        if (datos is null)
            return BadRequest(new { mensaje = "No se recibieron datos para generar el PDF." });

        var nombreActivo = string.IsNullOrWhiteSpace(datos.NombreActivo)
            ? "Activo sin nombre"
            : datos.NombreActivo;

        var categoria = string.IsNullOrWhiteSpace(datos.Categoria)
            ? "Sin categoría"
            : datos.Categoria;

        var dto = new ReporteDepreciacionDto
        {
            NombreActivo = nombreActivo,
            Categoria = categoria,
            ValorOriginal = datos.ValorOriginal,
            DescuentoMensual = datos.DescuentoMensual,
            DescuentoAcumulado = datos.DescuentoAcumulado,
            ValorActual = datos.ValorActual,
            FechaConsulta = datos.FechaConsulta,
            MesesTranscurridos = datos.MesesTranscurridos
        };

        var pdfBytes = _handler.Handle(dto);
        return File(pdfBytes, "application/pdf",
            $"depreciacion-{nombreActivo.Replace(" ", "_")}.pdf");
    }
}