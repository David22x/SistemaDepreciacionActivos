using ReportService.Domain.Models;

namespace ReportService.Application.Common.Interfaces;

public interface IPdfGenerator
{
    byte[] GenerarPdf(ReporteDepreciacionDto data);
}