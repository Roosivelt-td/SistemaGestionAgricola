import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { UserRegistration, UserLogin, AuthResponse } from '../types/auth.types';

interface AuthContextType {
  user: AuthResponse['user'] | null;
  token: string | null;
  isAuthenticated: boolean;
  register: (userData: UserRegistration) => Promise<void>;
  login: (credentials: UserLogin) => Promise<void>;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<AuthResponse['user'] | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  // Verificar si hay un token almacenado en localStorage al iniciar
  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');

    if (storedToken && storedUser) {
      setToken(storedToken);
      setUser(JSON.parse(storedUser));
    }
    
    setLoading(false);
  }, []);

  const register = async (userData: UserRegistration) => {
    // Simulación de registro - en una aplicación real, esto llamaría a una API
    setLoading(true);
    
    try {
      // Aquí iría la llamada a la API para registrar al usuario
      // const response = await authApi.register(userData);
      
      // Simulación de respuesta exitosa
      const mockResponse: AuthResponse = {
        token: 'mock-jwt-token-for-demo',
        user: {
          id: Math.random().toString(36).substr(2, 9),
          nombre: userData.nombre,
          apellido: userData.apellido,
          email: userData.email
        }
      };

      // Guardar en localStorage
      localStorage.setItem('token', mockResponse.token);
      localStorage.setItem('user', JSON.stringify(mockResponse.user));

      // Actualizar estado
      setToken(mockResponse.token);
      setUser(mockResponse.user);
    } catch (error) {
      console.error('Error en el registro:', error);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const login = async (credentials: UserLogin) => {
    // Simulación de inicio de sesión - en una aplicación real, esto llamaría a una API
    setLoading(true);
    
    try {
      // Aquí iría la llamada a la API para iniciar sesión
      // const response = await authApi.login(credentials);
      
      // Simulación de respuesta exitosa
      const mockResponse: AuthResponse = {
        token: 'mock-jwt-token-for-demo',
        user: {
          id: Math.random().toString(36).substr(2, 9),
          nombre: 'Usuario',
          apellido: 'Demo',
          email: credentials.email
        }
      };

      // Guardar en localStorage
      localStorage.setItem('token', mockResponse.token);
      localStorage.setItem('user', JSON.stringify(mockResponse.user));

      // Actualizar estado
      setToken(mockResponse.token);
      setUser(mockResponse.user);
    } catch (error) {
      console.error('Error en el inicio de sesión:', error);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    // Eliminar datos de autenticación
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    
    setToken(null);
    setUser(null);
  };

  const isAuthenticated = !!token;

  const value = {
    user,
    token,
    isAuthenticated,
    register,
    login,
    logout,
    loading
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};