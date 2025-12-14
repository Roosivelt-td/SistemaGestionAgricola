import axiosInstance from './axiosConfig';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../types/auth.types';

// Endpoint para login
export const login = (data: LoginRequest) => {
  return axiosInstance.post<LoginResponse>('/auth/login', data);
};

// Endpoint para registro
export const register = (data: RegisterRequest) => {
  return axiosInstance.post<RegisterResponse>('/auth/register', data);
};

// Endpoint para verificar token
export const verifyToken = () => {
  return axiosInstance.get('/auth/verify');
};

// Endpoint para logout
export const logout = () => {
  return axiosInstance.post('/auth/logout');
};