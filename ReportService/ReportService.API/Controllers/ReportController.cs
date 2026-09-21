using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportService.Domain.Models;
using ReportService.Infrastructure.Services;

namespace ReportService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly PdfGeneratorService _pdfService;

    public ReportController(PdfGeneratorService pdfService)
        => _pdfService = pdfService;

    [HttpPost("depreciacion/pdf")]
    public IActionResult GenerarPdfDepreciacion([FromBody] ReporteDepreciacionDto datos)
    {
        var pdfBytes = _pdfService.GenerarReporte(datos);
        return File(pdfBytes, "application/pdf",
            $"depreciacion-{datos.NombreActivo.Replace(" ", "_")}.pdf");
    }
}