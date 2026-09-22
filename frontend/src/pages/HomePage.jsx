import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AppLayout from '../components/AppLayout';
import LoadingState from '../components/LoadingState';
import { getActivos } from '../api/assetsApi';
import { formatMoney, formatDate } from '../utils/format';

export default function HomePage() {
  const [activos, setActivos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [busqueda, setBusqueda] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    getActivos()
      .then(({ data }) => setActivos(data))
      .finally(() => setLoading(false));
  }, []);

  const filtrados = activos.filter((a) =>
    a.nombre.toLowerCase().includes(busqueda.toLowerCase()));

  const valorTotal = activos.reduce((s, a) => s + a.valorOriginal, 0);
  const categorias = new Set(activos.map((a) => a.categoriaNombre)).size;

  return (
    <AppLayout
      title="Panel de activos"
      subtitle="Visualiza y consulta el valor depreciado de tus activos"
    >
      {/* KPIs */}
      <section className="stats-grid">
        <div className="stat-card">
          <span className="stat-label">Activos registrados</span>
          <span className="stat-value">{activos.length}</span>
        </div>
        <div className="stat-card">
          <span className="stat-label">Categorías en uso</span>
          <span className="stat-value">{categorias}</span>
        </div>
        <div className="stat-card">
          <span className="stat-label">Valor original total</span>
          <span className="stat-value">{formatMoney(valorTotal)}</span>
        </div>
      </section>

      {/* Búsqueda */}
      <div className="toolbar">
        <input
          className="input"
          placeholder="Buscar activo por nombre..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
        />
        <Link to="/activos/nuevo" className="btn btn-primary">
          + Nuevo activo
        </Link>
      </div>

      {/* Tabla */}
      {loading ? (
        <LoadingState texto="Cargando activos..." />
      ) : filtrados.length === 0 ? (
        <div className="empty-state">
          <h3>No hay activos</h3>
          <p>Registra tu primer activo para empezar a calcular depreciación.</p>
          <Link to="/activos/nuevo" className="btn btn-primary">
            Registrar activo
          </Link>
        </div>
      ) : (
        <div className="card-padded table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>Nombre</th>
                <th>Categoría</th>
                <th>Valor original</th>
                <th>Adquisición</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {filtrados.map((a) => (
                <tr key={a.id}>
                  <td>{a.nombre}</td>
                  <td><span className="badge">{a.categoriaNombre}</span></td>
                  <td>{formatMoney(a.valorOriginal)}</td>
                  <td>{formatDate(a.fechaAdquisicion)}</td>
                  <td className="row-actions">
                    <button
                      className="btn btn-secondary btn-sm"
                      onClick={() => navigate(`/consulta/${a.id}`)}
                    >
                      Consultar valor
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </AppLayout>
  );
}