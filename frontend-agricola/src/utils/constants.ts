// Constantes del sistema
export const APP_NAME = 'Sistema de Gestión Agrícola';
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:3000/api';
export const GOOGLE_CLIENT_ID = import.meta.env.VITE_GOOGLE_CLIENT_ID || '';

// Mensajes de error comunes
export const ERROR_MESSAGES = {
  NETWORK_ERROR: 'Error de conexión. Por favor, verifica tu conexión a internet.',
  SERVER_ERROR: 'Error del servidor. Inténtalo más tarde.',
  UNAUTHORIZED: 'No autorizado. Por favor, inicia sesión nuevamente.',
  VALIDATION_ERROR: 'Datos inválidos. Revisa los campos del formulario.'
};

// Mensajes de éxito comunes
export const SUCCESS_MESSAGES = {
  LOGIN_SUCCESS: 'Inicio de sesión exitoso',
  REGISTER_SUCCESS: 'Registro exitoso',
  LOGOUT_SUCCESS: 'Sesión cerrada exitosamente'
};

// Patrones de validación
export const REGEX_PATTERNS = {
  EMAIL: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
  PASSWORD: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
  NAME: /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{2,}$/
};