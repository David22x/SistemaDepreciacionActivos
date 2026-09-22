using ReportService.Application.Common.Interfaces;
using ReportService.Domain.Models;

namespace ReportService.Application.UseCases.GenerarReporteDepreciacion;

public class GenerarReporteDepreciacionHandler
{
    private readonly IPdfGenerator _pdf;
    public GenerarReporteDepreciacionHandler(IPdfGenerator pdf) => _pdf = pdf;

    public byte[] Handle(ReporteDepreciacionDto data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        return _pdf.GenerarPdf(data);
    }
}