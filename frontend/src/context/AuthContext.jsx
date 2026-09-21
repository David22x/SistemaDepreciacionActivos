import { createContext, useContext, useState, useEffect } from 'react';
import axiosClient from '../api/axiosClient';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [usuario, setUsuario] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      setUsuario({ token });
    }
    setLoading(false);
  }, []);

  const login = async (nombreUsuario, password) => {
    const { data } = await axiosClient.post('/auth/login', {
      nombreUsuario,
      password,
    });
    localStorage.setItem('token', data.token);
    setUsuario({ token: data.token });
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUsuario(null);
  };

  return (
    <AuthContext.Provider value={{ usuario, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);