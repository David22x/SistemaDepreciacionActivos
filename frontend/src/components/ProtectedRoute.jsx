import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/useAuth';

export default function ProtectedRoute({ children }) {
  const { usuario, loading } = useAuth();

  if (loading) return <div>Cargando...</div>;
  if (!usuario) return <Navigate to="/login" replace />;

  return children;
}