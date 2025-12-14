import axios from 'axios';
import { getToken, removeToken } from '../utils/auth';

// Configuración base de axios
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:3000/api';

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para agregar token a cada solicitud
axiosInstance.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor para manejar respuestas de error
axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    // Si el servidor responde con un error 401 (no autorizado)
    if (error.response?.status === 401) {
      // Eliminar el token almacenado
      removeToken();
      // Redirigir al login (esto podría hacerse mejor con el router)
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;