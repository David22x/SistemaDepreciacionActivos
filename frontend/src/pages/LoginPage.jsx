import { useState } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/useAuth';

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [form, setForm] = useState({ nombreUsuario: '', password: '' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await login(form.nombreUsuario, form.password);
      navigate('/');
    } catch (err) {
      setError(err?.response?.data?.mensaje || 'Credenciales inválidas');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-split">
      {/* Panel de marca (izquierda en desktop) */}
      <aside className="auth-brand">
        <div className="auth-brand-content">
          <span className="brand-mark brand-mark-lg">DA</span>
          <h1>Sistema de Análisis de Depreciación</h1>
          <p>
            Calcula el valor real de tus activos en cualquier fecha,
            aplicando normativa ecuatoriana de depreciación lineal.
          </p>
          <ul className="auth-brand-features">
            <li>✔ Depreciación mensual automática</li>
            <li>✔ Valor residual garantizado (10%)</li>
            <li>✔ Exportación a PDF e impresión</li>
          </ul>
        </div>
      </aside>

      {/* Formulario (derecha) */}
      <section className="auth-form-wrap">
        <form className="auth-card" onSubmit={handleSubmit}>
          <h2>Iniciar sesión</h2>
          <p className="page-subtitle">Bienvenido de nuevo</p>

          {location.state?.registrado && (
            <div className="alert-success">Cuenta creada. Ya puedes iniciar sesión.</div>
          )}
          {error && <div className="alert-error">{error}</div>}

          <div className="field">
            <label htmlFor="nombreUsuario">Usuario</label>
            <input
              id="nombreUsuario" name="nombreUsuario" className="input"
              value={form.nombreUsuario} onChange={handleChange}
              autoComplete="username" required
            />
          </div>

          <div className="field">
            <label htmlFor="password">Contraseña</label>
            <input
              id="password" name="password" type="password" className="input"
              value={form.password} onChange={handleChange}
              autoComplete="current-password" required
            />
          </div>

          <button className="btn btn-primary btn-block" disabled={loading}>
            {loading ? 'Ingresando...' : 'Ingresar'}
          </button>

          <p className="auth-footer">
            ¿No tienes cuenta? <Link to="/registro">Regístrate</Link>
          </p>
        </form>
      </section>
    </div>
  );
}