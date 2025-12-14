export interface UserRegistration {
  nombre: string;
  apellido: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface UserLogin {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: string;
    nombre: string;
    apellido: string;
    email: string;
  };
}