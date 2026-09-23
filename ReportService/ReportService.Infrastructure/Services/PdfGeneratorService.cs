using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ReportService.Application.Common.Interfaces;
using ReportService.Domain.Models;

namespace ReportService.Infrastructure.Services;

public class PdfGeneratorService : IPdfGenerator
{
    public byte[] GenerarPdf(ReporteDepreciacionDto data)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                // ---- HEADER ----
                page.Header().Column(col =>
                {
                    col.Item().Text("Reporte de Depreciación de Activos")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);
                    col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(10).FontColor(Colors.Grey.Medium);
                    col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                });

                // ---- CONTENT ----
                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Spacing(15);

                    // Datos generales
                    col.Item().Text("Datos del Activo").Bold().FontSize(14);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                        });

                        void Fila(string label, string valor)
                        {
                            table.Cell().Background(Colors.Grey.Lighten4)
                                .Padding(5).Text(label).Bold();
                            table.Cell().Padding(5).Text(valor);
                        }

                        Fila("Activo", data.NombreActivo);
                        Fila("Categoría", data.Categoria);
                        Fila("Fecha de consulta", data.FechaConsulta.ToString("dd/MM/yyyy"));
                        Fila("Meses transcurridos", data.MesesTranscurridos.ToString());
                    });

                    // Tabla de depreciación
                    col.Item().Text("Detalle de Depreciación").Bold().FontSize(14);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Concepto
                            columns.RelativeColumn(1); // Valor
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(6).Text("Concepto").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(6).Text("Valor (USD)").FontColor(Colors.White).Bold();
                        });

                        void FilaTabla(string concepto, decimal valor, bool destacar = false)
                        {
                            var bg = destacar ? Colors.Green.Lighten4 : Colors.White;
                            table.Cell().Background(bg).BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2).Padding(6).Text(concepto);
                            table.Cell().Background(bg).BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2).Padding(6)
                                .Text($"${valor:N2}").AlignRight();
                        }

                        FilaTabla("Valor original", data.ValorOriginal);
                        FilaTabla("Descuento por devalúo mensual", data.DescuentoMensual);
                        FilaTabla("Descuento acumulado", data.DescuentoAcumulado);
                        FilaTabla("Valor actual del activo", data.ValorActual, true);
                    });

                    if (data.DesglosePorAnio.Count > 0)
                    {
                        col.Item().Text("Desglose por año").Bold().FontSize(14);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                foreach (var titulo in new[] { "Año", "Inicio", "Descuento", "Acumulado", "Cierre" })
                                {
                                    header.Cell().Background(Colors.Blue.Darken2)
                                        .Padding(5).Text(titulo).FontColor(Colors.White).Bold();
                                }
                            });

                            foreach (var fila in data.DesglosePorAnio)
                            {
                                var valores = new[]
                                {
                                    fila.Anio.ToString(),
                                    $"${fila.ValorInicioAnio:N2}",
                                    $"${fila.DescuentoDelAnio:N2}",
                                    $"${fila.DescuentoAcumulado:N2}",
                                    $"${fila.ValorFinAnio:N2}"
                                };

                                foreach (var valor in valores)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .Padding(5).Text(valor);
                                }
                            }
                        });
                    }

                    // Nota
                    col.Item().Text("Nota: El valor residual mínimo es del 10% del valor original (normativa ecuatoriana).")
                        .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);
                });

                // ---- FOOTER ----
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }
}