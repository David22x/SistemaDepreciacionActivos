using DepreciationService.Domain.Models;

namespace DepreciationService.Domain.Services;

public class DepreciacionCalculator
{
    private const decimal ValorResidualPorcentaje = 0.10m;

    public DepreciacionResponse Calcular(ActivoDto activo, DateTime fechaConsulta)
    {
        var valorResidual = activo.ValorOriginal * ValorResidualPorcentaje;

        var depreciacionMensual = activo.VidaUtilMeses > 0
            ? (activo.ValorOriginal - valorResidual) / activo.VidaUtilMeses
            : 0m;

        var mesesTranscurridosTotal = CalcularMesesTranscurridos(activo.FechaAdquisicion, fechaConsulta);
        var mesesEfectivosTotal = Math.Min(mesesTranscurridosTotal, activo.VidaUtilMeses);
        var descuentoAcumuladoTotal = depreciacionMensual * mesesEfectivosTotal;

        var valorActualTotal = activo.ValorOriginal - descuentoAcumuladoTotal;
        if (valorActualTotal < valorResidual) valorActualTotal = valorResidual;

        return new DepreciacionResponse
        {
            Activo = activo.Nombre,
            CategoriaNombre = string.IsNullOrWhiteSpace(activo.CategoriaNombre) ? "Sin categoría" : activo.CategoriaNombre,
            ValorOriginal = activo.ValorOriginal,
            DescuentoPorDevaluo = Math.Round(depreciacionMensual, 2),
            DescuentoAcumulado = Math.Round(descuentoAcumuladoTotal, 2),
            ValorActual = Math.Round(valorActualTotal, 2),
            MesesTranscurridos = mesesTranscurridosTotal,
            FechaAdquisicion = activo.FechaAdquisicion,
            FechaConsulta = fechaConsulta,
            DesglosePorAnio = ConstruirDesglosePorAnio(activo, fechaConsulta, depreciacionMensual, valorResidual)
        };
    }

    private static List<DepreciacionAnualDto> ConstruirDesglosePorAnio(
        ActivoDto activo, DateTime fechaConsulta, decimal depreciacionMensual, decimal valorResidual)
    {
        var desglose = new List<DepreciacionAnualDto>();

        if (fechaConsulta < activo.FechaAdquisicion)
        {
            desglose.Add(new DepreciacionAnualDto
            {
                Anio = activo.FechaAdquisicion.Year,
                ValorInicioAnio = activo.ValorOriginal,
                DescuentoDelAnio = 0m,
                DescuentoAcumulado = 0m,
                ValorFinAnio = activo.ValorOriginal
            });
            return desglose;
        }

        var valorInicioAnio = activo.ValorOriginal;

        for (var anio = activo.FechaAdquisicion.Year; anio <= fechaConsulta.Year; anio++)
        {
            var esUltimoAnio = anio == fechaConsulta.Year;
            var fechaReferencia = esUltimoAnio ? fechaConsulta : new DateTime(anio, 12, 31);

            var meses = Math.Min(CalcularMesesTranscurridos(activo.FechaAdquisicion, fechaReferencia), activo.VidaUtilMeses);
            var descuentoAcumulado = depreciacionMensual * meses;

            var valorFinAnio = activo.ValorOriginal - descuentoAcumulado;
            if (valorFinAnio < valorResidual) valorFinAnio = valorResidual;

            desglose.Add(new DepreciacionAnualDto
            {
                Anio = anio,
                ValorInicioAnio = Math.Round(valorInicioAnio, 2),
                DescuentoDelAnio = Math.Round(valorInicioAnio - valorFinAnio, 2),
                DescuentoAcumulado = Math.Round(descuentoAcumulado, 2),
                ValorFinAnio = Math.Round(valorFinAnio, 2)
            });

            valorInicioAnio = valorFinAnio;
        }

        return desglose;
    }

    private static int CalcularMesesTranscurridos(DateTime adquisicion, DateTime consulta)
    {
        if (consulta < adquisicion) return 0;
        var meses = (consulta.Year - adquisicion.Year) * 12 + (consulta.Month - adquisicion.Month);
        if (consulta.Day < adquisicion.Day) meses--;
        return Math.Max(0, meses);
    }
}