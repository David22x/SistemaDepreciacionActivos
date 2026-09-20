using DepreciationService.Domain.Models;

namespace DepreciationService.Domain.Services;

public class DepreciacionCalculator
{
    private const decimal ValorResidualPorcentaje = 0.10m;

    public DepreciacionResponse Calcular(ActivoDto activo, DateTime fechaConsulta)
    {
        // 1. Valor residual = 10% del valor original
        var valorResidual = activo.ValorOriginal * ValorResidualPorcentaje;

        // 2. Depreciación mensual = (valor original − valor residual) / vida útil
        var depreciacionMensual = activo.VidaUtilMeses > 0
            ? (activo.ValorOriginal - valorResidual) / activo.VidaUtilMeses
            : 0m;

        // 3. Meses transcurridos desde la adquisición
        var mesesTranscurridos = CalcularMesesTranscurridos(
            activo.FechaAdquisicion, fechaConsulta);

        // 4. Descuento acumulado (no puede exceder la vida útil)
        var mesesEfectivos = Math.Min(mesesTranscurridos, activo.VidaUtilMeses);
        var descuentoAcumulado = depreciacionMensual * mesesEfectivos;

        // 5. Valor actual con piso del 10%
        var valorActual = activo.ValorOriginal - descuentoAcumulado;
        if (valorActual < valorResidual)
            valorActual = valorResidual;

        return new DepreciacionResponse
        {
            Activo = activo.Nombre,
            ValorOriginal = activo.ValorOriginal,
            DescuentoPorDevaluo = Math.Round(depreciacionMensual, 2),
            DescuentoAcumulado = Math.Round(descuentoAcumulado, 2),
            ValorActual = Math.Round(valorActual, 2),
            MesesTranscurridos = mesesTranscurridos,
            FechaConsulta = fechaConsulta
        };
    }

    /// <summary>
    /// Calcula meses completos transcurridos entre dos fechas.
    /// Si la fecha de consulta es anterior a la adquisición, retorna 0.
    /// </summary>
    private static int CalcularMesesTranscurridos(DateTime adquisicion, DateTime consulta)
    {
        if (consulta < adquisicion) return 0;

        var meses = (consulta.Year - adquisicion.Year) * 12
                    + (consulta.Month - adquisicion.Month);

        // Ajuste por día: si el día de consulta es menor al día de adquisición,
        // aún no se completa el mes en curso.
        if (consulta.Day < adquisicion.Day) meses--;

        return Math.Max(0, meses);
    }
}