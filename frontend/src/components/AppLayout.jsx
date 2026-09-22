import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/useAuth';

export default function AppLayout({ children, title, subtitle, actions }) {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="app-shell">
      <header className="topbar no-print">
        <Link to="/" className="brand">
          <span className="brand-mark">DA</span>
          <span className="brand-text">Depreciación de Activos</span>
        </Link>

        <nav className="topbar-nav">
          <Link to="/" className="btn btn-ghost btn-sm">Inicio</Link>
          <Link to="/activos/nuevo" className="btn btn-primary btn-sm">
            + Nuevo activo
          </Link>
          <button className="btn btn-ghost btn-sm" onClick={handleLogout}>
            Cerrar sesión
          </button>
        </nav>
      </header>

      <main className="page-content">
        {(title || actions) && (
          <div className="page-header">
            <div>
              {title && <h1 className="page-title">{title}</h1>}
              {subtitle && <p className="page-subtitle">{subtitle}</p>}
            </div>
            {actions && <div className="page-actions">{actions}</div>}
          </div>
        )}
        {children}
      </main>
    </div>
  );
}