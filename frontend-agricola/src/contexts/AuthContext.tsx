import React, { createContext, useContext, useState, useEffect } from 'react';
import { User } from '../types/auth.types';
import { login, register } from '../api/authApi';
import { saveToken, getToken, removeToken } from '../utils/auth';

interface AuthContextType {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, password: string, nombre: string, apellido: string) => Promise<void>;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Verificar si hay un token almacenado al cargar la aplicación
    const storedToken = getToken();
    if (storedToken) {
      // En una implementación real, aquí haríamos una llamada para verificar el token
      // Por ahora, simplemente establecemos el estado basado en el token almacenado
      setToken(storedToken);
      // Aquí normalmente obtendríamos la información del usuario desde el token
    }
    setLoading(false);
  }, []);

  const loginUser = async (email: string, password: string) => {
    setLoading(true);
    try {
      const response = await login(email, password);
      const { token: accessToken, refreshToken, user: userData } = response.data;

      saveToken(accessToken, refreshToken);
      setToken(accessToken);
      setUser(userData);
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Error en el inicio de sesión');
    } finally {
      setLoading(false);
    }
  };

  const registerUser = async (email: string, password: string, nombre: string, apellido: string) => {
    setLoading(true);
    try {
      const response = await register(email, password, nombre, apellido);
      
      if(response.data.success) {
        // Si el registro es exitoso, iniciar sesión automáticamente
        await loginUser(email, password);
      } else {
        throw new Error(response.data.message || 'Error en el registro');
      }
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Error en el registro');
    } finally {
      setLoading(false);
    }
  };

  const logoutUser = () => {
    removeToken();
    setToken(null);
    setUser(null);
  };

  const value: AuthContextType = {
    user,
    token,
    isAuthenticated: !!token,
    login: loginUser,
    register: registerUser,
    logout: logoutUser,
    loading,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth debe usarse dentro de un AuthProvider');
  }
  return context;
};