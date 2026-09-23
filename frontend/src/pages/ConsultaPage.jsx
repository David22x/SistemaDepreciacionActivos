import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import AppLayout from '../components/AppLayout';
import { consultarDepreciacion } from '../api/depreciationApi';
import { generarPdf } from '../api/reportApi';
import { formatMoney, formatDate } from '../utils/format';

export default function ConsultaPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [fecha, setFecha] = useState(new Date().toISOString().split('T')[0]);
  const [resultado, setResultado] = useState(null);
  const [loading, setLoading] = useState(false);
  const [exportando, setExportando] = useState(false);
  const [error, setError] = useState('');

  const consultar = async () => {
    setLoading(true);
    setError('');
    try {
      const { data } = await consultarDepreciacion(Number(id), fecha);
      setResultado({
        nombreActivo: data.activo,
        categoriaNombre: data.categoriaNombre ?? 'Sin categoría',
        valorOriginal: data.valorOriginal,
        descuentoMensual: data.descuentoPorDevaluo,
        descuentoAcumulado: data.descuentoAcumulado,
        valorActual: data.valorActual,
        fechaConsulta: data.fechaConsulta,
        fechaAdquisicion: data.fechaAdquisicion,
        mesesTranscurridos: data.mesesTranscurridos,
        desglosePorAnio: data.desglosePorAnio ?? [],
      });
    } catch (err) {
      setError(err.response?.data?.mensaje || 'Error al consultar la depreciación');
    } finally {
      setLoading(false);
    }
  };

  const exportarPdf = async () => {
    if (!resultado) return;
    setError('');
    setExportando(true);
    try {
      const response = await generarPdf({
        nombreActivo: resultado.nombreActivo,
        categoria: resultado.categoriaNombre,
        valorOriginal: resultado.valorOriginal,
        descuentoMensual: resultado.descuentoMensual,
        descuentoAcumulado: resultado.descuentoAcumulado,
        valorActual: resultado.valorActual,
        fechaConsulta: resultado.fechaConsulta,
        mesesTranscurridos: resultado.mesesTranscurridos,
        desglosePorAnio: resultado.desglosePorAnio,
      });
      const url = window.URL.createObjectURL(new Blob([response.data], { type: 'application/pdf' }));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `depreciacion-activo-${id}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch (err) {
      setError(err.response?.data?.mensaje || 'Error al generar el PDF');
    } finally {
      setExportando(false);
    }
  };

  return (
    <AppLayout
      title="Consulta de valor depreciado"
      subtitle="Selecciona una fecha para ver el valor del activo en ese momento"
      actions={
        <button className="btn btn-ghost btn-sm no-print" onClick={() => navigate('/')}>
          ← Volver al listado
        </button>
      }
    >
      <div className="card-padded no-print" style={{ marginBottom: 'var(--space-5)' }}>
        <div style={{ display: 'flex', alignItems: 'flex-end', gap: 'var(--space-4)', flexWrap: 'wrap' }}>
          <div className="field">
            <label htmlFor="fecha">Fecha de consulta</label>
            <input
              id="fecha"
              className="input"
              type="date"
              value={fecha}
              onChange={(e) => setFecha(e.target.value)}
            />
          </div>
          <button className="btn btn-primary" onClick={consultar} disabled={loading}>
            {loading ? 'Consultando...' : 'Consultar'}
          </button>
        </div>
      </div>

      {error && <div className="alert alert-error" style={{ marginBottom: 'var(--space-4)' }}>{error}</div>}

      {resultado && (
        <div className="card-padded">
          <div style={{ display: 'flex', justifyContent: 'space-between', flexWrap: 'wrap', gap: 12 }}>
            <div>
              <h2 style={{ margin: 0 }}>{resultado.nombreActivo}</h2>
              <span className="badge">{resultado.categoriaNombre}</span>
            </div>
            <div style={{ textAlign: 'right' }}>
              <div style={{ fontSize: '0.8rem', color: 'var(--color-text-muted)' }}>
                Valor actual ({formatDate(resultado.fechaConsulta)})
              </div>
              <div style={{ fontSize: '1.4rem', fontWeight: 700, color: 'var(--color-success)' }}>
                {formatMoney(resultado.valorActual)}
              </div>
            </div>
          </div>

          <section className="stats-grid" style={{ marginTop: 'var(--space-5)' }}>
            <div className="stat-card">
              <span className="stat-label">Fecha de adquisición</span>
              <span className="stat-value" style={{ fontSize: '1rem' }}>{formatDate(resultado.fechaAdquisicion)}</span>
            </div>
            <div className="stat-card">
              <span className="stat-label">Valor original</span>
              <span className="stat-value" style={{ fontSize: '1rem' }}>{formatMoney(resultado.valorOriginal)}</span>
            </div>
            <div className="stat-card">
              <span className="stat-label">Descuento acumulado</span>
              <span className="stat-value" style={{ fontSize: '1rem' }}>{formatMoney(resultado.descuentoAcumulado)}</span>
            </div>
          </section>

          <h3 style={{ marginTop: 'var(--space-5)', marginBottom: 4 }}>Desglose por año</h3>
          <p style={{ color: 'var(--color-text-muted)', fontSize: '0.85rem', marginTop: 0 }}>
            Valor del activo al cierre de cada año, desde su adquisición hasta la fecha consultada.
          </p>

          <div className="table-wrap" style={{ marginTop: 'var(--space-2)' }}>
            <table className="data-table">
              <thead>
                <tr>
                  <th>Año</th>
                  <th style={{ textAlign: 'right' }}>Valor inicio de año</th>
                  <th style={{ textAlign: 'right' }}>Descuento del año</th>
                  <th style={{ textAlign: 'right' }}>Descuento acumulado</th>
                  <th style={{ textAlign: 'right' }}>Valor al cierre</th>
                </tr>
              </thead>
              <tbody>
                {resultado.desglosePorAnio.map((fila, idx) => {
                  const esUltimo = idx === resultado.desglosePorAnio.length - 1;
                  return (
                    <tr key={fila.anio} style={esUltimo ? { background: '#f0fdf4' } : undefined}>
                      <td>{fila.anio}</td>
                      <td style={{ textAlign: 'right' }}>{formatMoney(fila.valorInicioAnio)}</td>
                      <td style={{ textAlign: 'right' }}>{formatMoney(fila.descuentoDelAnio)}</td>
                      <td style={{ textAlign: 'right' }}>{formatMoney(fila.descuentoAcumulado)}</td>
                      <td style={{ textAlign: 'right' }}><strong>{formatMoney(fila.valorFinAnio)}</strong></td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>

          <div className="no-print" style={{ display: 'flex', justifyContent: 'flex-end', gap: 'var(--space-3)', marginTop: 'var(--space-5)' }}>
            <button className="btn btn-secondary" onClick={() => window.print()}>🖨️ Imprimir</button>
            <button className="btn btn-primary" onClick={exportarPdf} disabled={exportando}>
              {exportando ? 'Generando PDF...' : '📄 Exportar PDF'}
            </button>
          </div>
        </div>
      )}
    </AppLayout>
  );
}