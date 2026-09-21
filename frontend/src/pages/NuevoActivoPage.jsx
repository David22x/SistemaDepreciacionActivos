import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function NuevoActivoPage() {
  const [form, setForm] = useState({
    nombre: '',
    valorOriginal: '',
    fechaAdquisicion: '',
    categoriaId: '',
  });
  const [categorias, setCategorias] = useState([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    axiosClient.get('/assets/categorias')
      .then(({ data }) => setCategorias(data))
      .catch(console.error);
  }, []);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      await axiosClient.post('/assets', {
        nombre: form.nombre,
        valorOriginal: parseFloat(form.valorOriginal),
        fechaAdquisicion: form.fechaAdquisicion,
        categoriaId: parseInt(form.categoriaId),
      });
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.mensaje || 'Error al crear el activo');
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h2>Registrar Nuevo Activo</h2>
        {error && <p style={styles.error}>{error}</p>}

        <form onSubmit={handleSubmit} style={styles.form}>
          <label style={styles.label}>Nombre del activo</label>
          <input style={styles.input} name="nombre" value={form.nombre} onChange={handleChange} required />

          <label style={styles.label}>Valor original (USD)</label>
          <input style={styles.input} name="valorOriginal" type="number" step="0.01" value={form.valorOriginal} onChange={handleChange} required />

          <label style={styles.label}>Fecha de adquisición</label>
          <input style={styles.input} name="fechaAdquisicion" type="date" value={form.fechaAdquisicion} onChange={handleChange} required />

          <label style={styles.label}>Categoría</label>
          <select style={styles.input} name="categoriaId" value={form.categoriaId} onChange={handleChange} required>
            <option value="">Selecciona una categoría</option>
            {categorias.map((cat) => (
              <option key={cat.id} value={cat.id}>{cat.nombre} — {cat.vidaUtilAnios} años</option>
            ))}
          </select>

          <div style={styles.actions}>
            <button type="button" style={styles.cancelBtn} onClick={() => navigate('/')}>
              Cancelar
            </button>
            <button type="submit" style={styles.submitBtn}>
              Guardar Activo
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

const styles = {
  container: { display: 'flex', justifyContent: 'center', padding: '2rem', minHeight: '100vh', background: '#f0f2f5' },
  card: { background: '#fff', padding: '2rem', borderRadius: '12px', boxShadow: '0 4px 12px rgba(0,0,0,0.1)', width: '480px' },
  form: { display: 'flex', flexDirection: 'column', gap: '0.75rem' },
  label: { fontWeight: 'bold', fontSize: '0.9rem', color: '#333' },
  input: { padding: '0.6rem', borderRadius: '8px', border: '1px solid #ddd', fontSize: '1rem' },
  actions: { display: 'flex', justifyContent: 'flex-end', gap: '1rem', marginTop: '1rem' },
  cancelBtn: { padding: '0.6rem 1.2rem', borderRadius: '8px', border: '1px solid #ddd', background: '#fff', cursor: 'pointer' },
  submitBtn: { padding: '0.6rem 1.2rem', borderRadius: '8px', border: 'none', background: '#1a1a2e', color: '#fff', cursor: 'pointer' },
  error: { color: '#e74c3c', fontSize: '0.9rem' },
};