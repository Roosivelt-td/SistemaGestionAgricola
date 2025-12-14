export interface User {
  id: string;
  email: string;
  nombre: string;
  apellido: string;
  rol: string;
  fechaCreacion: string;
  ultimoAcceso: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  user: User;
}

export interface RegisterRequest {
  email: string;
  password: string;
  nombre: string;
  apellido: string;
}

export interface RegisterResponse {
  success: boolean;
  message: string;
  user?: User;
}