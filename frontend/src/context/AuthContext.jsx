import { useState } from 'react';
import axiosClient from '../api/axiosClient';

import { AuthContext } from './AuthContextValue';

export function AuthProvider({ children }) {
  const [usuario, setUsuario] = useState(() => {
    const token = localStorage.getItem('token');
    return token ? { token } : null;
  });

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
    <AuthContext.Provider value={{ usuario, login, logout, loading: false }}>
      {children}
    </AuthContext.Provider>
  );
}
