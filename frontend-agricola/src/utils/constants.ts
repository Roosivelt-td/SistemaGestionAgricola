// Constantes del sistema
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:3000/api';

export const ROUTES = {
  LOGIN: '/login',
  REGISTER: '/register',
  DASHBOARD: '/dashboard',
  HOME: '/',
} as const;

export const PASSWORD_REQUIREMENTS = [
  'Al menos 8 caracteres',
  'Una letra mayúscula',
  'Una letra minúscula',
  'Un número',
  'Un carácter especial (@$!%*?&)'
];