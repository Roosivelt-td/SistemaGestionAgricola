import React from 'react';
import RegisterForm from '../components/auth/RegisterForm';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { ROUTES } from '../utils/constants';

const RegisterPage: React.FC = () => {
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleRegister = async (userData: { 
    nombre: string; 
    apellido: string; 
    email: string; 
    password: string; 
    confirmPassword: string; 
  }) => {
    try {
      await register({
        nombre: userData.nombre,
        apellido: userData.apellido,
        email: userData.email,
        password: userData.password,
        confirmPassword: userData.confirmPassword
      });
      
      // Redirigir al dashboard después del registro exitoso
      navigate(ROUTES.DASHBOARD);
    } catch (error) {
      console.error('Error en el registro:', error);
      // Aquí se podría manejar el error mostrando un mensaje al usuario
    }
  };

  return (
    <div className="register-page">
      <RegisterForm onRegister={handleRegister} />
    </div>
  );
};

export default RegisterPage;