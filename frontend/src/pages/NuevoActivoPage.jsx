import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';
import AppLayout from '../components/AppLayout';

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
    <AppLayout
      title="Registrar nuevo activo"
      subtitle="Completa los datos para incorporar un activo al sistema"
      actions={<button className="btn btn-ghost btn-sm" onClick={() => navigate('/')}>← Volver al listado</button>}
    >
      <div className="card-padded asset-form-card">
        {error && <div className="alert alert-error error-message">{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className="field">
            <label htmlFor="nombre">Nombre del activo</label>
            <input className="input" id="nombre" name="nombre" value={form.nombre} onChange={handleChange} required />
          </div>

          <div className="field">
            <label htmlFor="valorOriginal">Valor original (USD)</label>
            <input className="input" id="valorOriginal" name="valorOriginal" type="number" step="0.01" value={form.valorOriginal} onChange={handleChange} required />
          </div>

          <div className="field">
            <label htmlFor="fechaAdquisicion">Fecha de adquisición</label>
            <input className="input" id="fechaAdquisicion" name="fechaAdquisicion" type="date" value={form.fechaAdquisicion} onChange={handleChange} required />
          </div>

          <div className="field">
            <label htmlFor="categoriaId">Categoría</label>
            <select className="input" id="categoriaId" name="categoriaId" value={form.categoriaId} onChange={handleChange} required>
              <option value="">Selecciona una categoría</option>
              {categorias.map((cat) => {
                const anios = cat.vidaUtilMeses ? Math.round(cat.vidaUtilMeses / 12) : 0;
                return (
                  <option key={cat.id} value={cat.id}>{cat.nombre} - {anios} años</option>
                );
              })}
            </select>
          </div>

          <div className="form-actions">
            <button type="button" className="btn btn-secondary" onClick={() => navigate('/')}>
              Cancelar
            </button>
            <button type="submit" className="btn btn-primary">
              Guardar Activo
            </button>
          </div>
        </form>
      </div>
    </AppLayout>
  );
}