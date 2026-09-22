import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';
import { formatMoney, formatDate } from '../utils/format';

export default function ConsultaPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [fecha, setFecha] = useState(new Date().toISOString().split('T')[0]);
  const [resultado, setResultado] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const consultar = async () => {
    setLoading(true);
    setError('');
    try {
      const { data } = await axiosClient.post('/depreciation/calcular', {
        activoId: Number(id),
        fechaConsulta: fecha,
      });

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
      });
    } catch (err) {
      setError(err.response?.data?.mensaje || 'Error al consultar la depreciación');
    } finally {
      setLoading(false);
    }
  };

  const exportarPdf = async () => {
    if (!resultado) return;
    try {
      const response = await axiosClient.post(
        '/reports/depreciacion/pdf',
        {
          nombreActivo: resultado.nombreActivo,
          categoria: resultado.categoriaNombre,
          valorOriginal: resultado.valorOriginal,
          descuentoMensual: resultado.descuentoMensual,
          descuentoAcumulado: resultado.descuentoAcumulado,
          valorActual: resultado.valorActual,
          fechaConsulta: resultado.fechaConsulta,
          mesesTranscurridos: resultado.mesesTranscurridos,
        },
        { responseType: 'blob' }
      );

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `depreciacion-${id}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (err) {
      setError(err.response?.data?.mensaje || 'Error al generar el PDF');
    }
  };

  const imprimir = () => window.print();

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <button style={styles.backBtn} onClick={() => navigate('/')}>
          ← Volver al listado
        </button>

        <h2 style={styles.title}>Consulta de Valor Depreciado</h2>

        <div style={styles.controls}>
          <div style={styles.inlineField}>
            <label style={styles.label}>Fecha de consulta:</label>
            <input
              style={styles.input}
              type="date"
              value={fecha}
              onChange={(e) => setFecha(e.target.value)}
            />
          </div>
          <button style={styles.consultarBtn} onClick={consultar} disabled={loading}>
            {loading ? 'Consultando...' : 'Consultar'}
          </button>
        </div>

        {error && <p style={styles.error}>{error}</p>}

        {resultado && (
          <>
          <div className="consulta-hero">
  <div>
    <p className="consulta-hero-label">Valor actual del activo</p>
    <p className="consulta-hero-value">{formatMoney(resultado.valorActual)}</p>
    <p className="consulta-hero-sub">
      {resultado.nombreActivo} · {resultado.categoriaNombre}
    </p>
  </div>
</div>

<section className="stats-grid">
  <div className="stat-card">
    <span className="stat-label">Fecha de adquisición</span>
    <span className="stat-value">{formatDate(resultado.fechaAdquisicion)}</span>
  </div>
  <div className="stat-card">
    <span className="stat-label">Valor original</span>
    <span className="stat-value">{formatMoney(resultado.valorOriginal)}</span>
  </div>
  <div className="stat-card">
    <span className="stat-label">Descuento acumulado</span>
    <span className="stat-value">
      {formatMoney(resultado.descuentoAcumulado)}
    </span>
  </div>
</section>
            <table style={styles.table}>
              <thead>
                <tr>
                  <th style={styles.th}>Concepto</th>
                  <th style={styles.th}>Valor</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td style={styles.td}>Activo</td>
                  <td style={styles.td}>{resultado.nombreActivo}</td>
                </tr>
                <tr>
                  <td style={styles.td}>Categoría</td>
                  <td style={styles.td}>{resultado.categoriaNombre}</td>
                </tr>
                <tr>
                  <td style={styles.td}>Fecha de adquisición</td>
                  <td style={styles.td}>{resultado.fechaAdquisicion ? new Date(resultado.fechaAdquisicion).toLocaleDateString() : 'No disponible'}</td>
                </tr>
                <tr>
                  <td style={styles.td}>Valor original</td>
                  <td style={styles.td}>${resultado.valorOriginal?.toLocaleString()}</td>
                </tr>
                <tr>
                  <td style={styles.td}>Descuento por devalúo (mensual)</td>
                  <td style={styles.td}>${resultado.descuentoMensual?.toLocaleString()}</td>
                </tr>
                <tr>
                  <td style={styles.td}>Descuento acumulado</td>
                  <td style={styles.td}>${resultado.descuentoAcumulado?.toLocaleString()}</td>
                </tr>
                <tr style={styles.highlightRow}>
                  <td style={styles.td}><strong>Valor actual del activo</strong></td>
                  <td style={styles.td}><strong>${resultado.valorActual?.toLocaleString()}</strong></td>
                </tr>
              </tbody>
            </table>

            <div style={styles.actions}>
              <button style={styles.pdfBtn} onClick={exportarPdf}>
                📄 Exportar PDF
              </button>
              <button style={styles.printBtn} onClick={imprimir}>
                🖨️ Imprimir
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
}

const styles = {
  container: { display: 'flex', justifyContent: 'center', padding: '2rem', minHeight: '100vh', background: '#f0f2f5' },
  card: { background: '#fff', padding: '2rem', borderRadius: '12px', boxShadow: '0 4px 12px rgba(0,0,0,0.1)', width: '600px' },
  backBtn: { marginBottom: '1rem', padding: '0.4rem 0.8rem', border: 'none', background: 'transparent', color: '#3498db', cursor: 'pointer', fontSize: '1rem' },
  title: { marginTop: 0, color: '#1a1a2e' },
  controls: { display: 'flex', alignItems: 'flex-end', flexWrap: 'wrap', gap: '1rem', marginBottom: '1.5rem' },
  inlineField: { display: 'flex', flexDirection: 'column', gap: '0.35rem' },
  label: { fontWeight: 'bold', fontSize: '0.9rem' },
  input: { padding: '0.5rem', borderRadius: '8px', border: '1px solid #ddd' },
  consultarBtn: { padding: '0.5rem 1.2rem', borderRadius: '8px', border: 'none', background: '#3498db', color: '#fff', cursor: 'pointer' },
  table: { width: '100%', borderCollapse: 'collapse', marginBottom: '1.5rem' },
  th: { padding: '0.75rem 1rem', background: '#1a1a2e', color: '#fff', textAlign: 'left' },
  td: { padding: '0.75rem 1rem', borderBottom: '1px solid #eee' },
  highlightRow: { background: '#e8f8f0' },
  actions: { display: 'flex', gap: '1rem', justifyContent: 'flex-end' },
  pdfBtn: { padding: '0.6rem 1.2rem', borderRadius: '8px', border: 'none', background: '#e74c3c', color: '#fff', cursor: 'pointer' },
  printBtn: { padding: '0.6rem 1.2rem', borderRadius: '8px', border: 'none', background: '#1a1a2e', color: '#fff', cursor: 'pointer' },
  error: { color: '#e74c3c', marginBottom: '1rem' },
};