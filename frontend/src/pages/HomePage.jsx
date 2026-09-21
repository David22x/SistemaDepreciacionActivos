import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';
import { useAuth } from '../context/AuthContext';

export default function HomePage() {
  const [activos, setActivos] = useState([]);
  const [loading, setLoading] = useState(true);
  const { logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    axiosClient.get('/assets')
      .then(({ data }) => setActivos(data))
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (loading) return <p style={{ textAlign: 'center', marginTop: '2rem' }}>Cargando activos...</p>;

  return (
    <div style={styles.container}>
      <nav style={styles.navbar}>
        <h1 style={styles.navTitle}>Depreciación de Activos</h1>
        <div>
          <button style={styles.navBtn} onClick={() => navigate('/activos/nuevo')}>
            + Nuevo Activo
          </button>
          <button style={{ ...styles.navBtn, background: '#e74c3c' }} onClick={handleLogout}>
            Cerrar sesión
          </button>
        </div>
      </nav>

      <main style={styles.main}>
        <h2>Listado de Activos</h2>

        {activos.length === 0 ? (
          <p>No hay activos registrados. ¡Crea el primero!</p>
        ) : (
          <table style={styles.table}>
            <thead>
              <tr>
                <th style={styles.th}>Nombre</th>
                <th style={styles.th}>Categoría</th>
                <th style={styles.th}>Valor Original</th>
                <th style={styles.th}>Fecha Adquisición</th>
                <th style={styles.th}>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {activos.map((activo) => (
                <tr key={activo.id}>
                  <td style={styles.td}>{activo.nombre}</td>
                  <td style={styles.td}>{activo.categoriaNombre}</td>
                  <td style={styles.td}>${activo.valorOriginal?.toLocaleString()}</td>
                  <td style={styles.td}>{new Date(activo.fechaAdquisicion).toLocaleDateString()}</td>
                  <td style={styles.td}>
                    <button
                      style={styles.actionBtn}
                      onClick={() => navigate(`/consulta/${activo.id}`)}
                    >
                      Consultar valor
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </main>
    </div>
  );
}

const styles = {
  container: { minHeight: '100vh', background: '#f0f2f5' },
  navbar: { display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '1rem 2rem', background: '#1a1a2e', color: '#fff' },
  navTitle: { margin: 0, fontSize: '1.2rem' },
  navBtn: { marginLeft: '1rem', padding: '0.5rem 1rem', borderRadius: '6px', border: 'none', background: '#3498db', color: '#fff', cursor: 'pointer' },
  main: { padding: '2rem' },
  table: { width: '100%', borderCollapse: 'collapse', background: '#fff', borderRadius: '8px', overflow: 'hidden', boxShadow: '0 2px 8px rgba(0,0,0,0.08)' },
  th: { padding: '0.75rem 1rem', background: '#1a1a2e', color: '#fff', textAlign: 'left' },
  td: { padding: '0.75rem 1rem', borderBottom: '1px solid #eee' },
  actionBtn: { padding: '0.4rem 0.8rem', borderRadius: '6px', border: 'none', background: '#27ae60', color: '#fff', cursor: 'pointer' },
};