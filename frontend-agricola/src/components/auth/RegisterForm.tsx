import React, { useState } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { getErrorMessage, validateEmail, validatePassword, validateName } from '../../utils/validators';
import { GoogleLogin } from '@react-oauth/google';

interface FormData {
  email: string;
  password: string;
  confirmPassword: string;
  nombre: string;
  apellido: string;
}

const RegisterForm: React.FC<{ onSwitchToLogin: () => void }> = ({ onSwitchToLogin }) => {
  const { register, loading } = useAuth();
  const [formData, setFormData] = useState<FormData>({
    email: '',
    password: '',
    confirmPassword: '',
    nombre: '',
    apellido: ''
  });
  const [errors, setErrors] = useState<Partial<FormData>>({});
  const [generalError, setGeneralError] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    // Limpiar error cuando se empieza a escribir
    if (errors[name as keyof FormData]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
    
    // Validar en tiempo real
    if (name === 'email') {
      if (value && !validateEmail(value)) {
        setErrors(prev => ({
          ...prev,
          email: 'Por favor ingrese un correo electrónico válido'
        }));
      }
    } else if (name === 'password') {
      if (value && !validatePassword(value)) {
        setErrors(prev => ({
          ...prev,
          password: 'La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial'
        }));
      }
    } else if (name === 'nombre' || name === 'apellido') {
      if (value && !validateName(value)) {
        setErrors(prev => ({
          ...prev,
          [name]: 'Por favor ingrese un nombre válido (solo letras)'
        }));
      }
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Partial<FormData> = {};

    // Validar campos requeridos
    if (!formData.email.trim()) {
      newErrors.email = 'El correo electrónico es obligatorio';
    } else if (!validateEmail(formData.email)) {
      newErrors.email = 'Por favor ingrese un correo electrónico válido';
    }

    if (!formData.password.trim()) {
      newErrors.password = 'La contraseña es obligatoria';
    } else if (!validatePassword(formData.password)) {
      newErrors.password = 'La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial';
    }

    if (!formData.confirmPassword.trim()) {
      newErrors.confirmPassword = 'Debe confirmar la contraseña';
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Las contraseñas no coinciden';
    }

    if (!formData.nombre.trim()) {
      newErrors.nombre = 'El nombre es obligatorio';
    } else if (!validateName(formData.nombre)) {
      newErrors.nombre = 'Por favor ingrese un nombre válido (solo letras)';
    }

    if (!formData.apellido.trim()) {
      newErrors.apellido = 'El apellido es obligatorio';
    } else if (!validateName(formData.apellido)) {
      newErrors.apellido = 'Por favor ingrese un apellido válido (solo letras)';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      setGeneralError('');
      await register(
        formData.email,
        formData.password,
        formData.nombre,
        formData.apellido
      );
    } catch (error: any) {
      setGeneralError(error.message || 'Error en el registro');
    }
  };

  // Manejador para login con Google
  const handleGoogleSuccess = async (credentialResponse: any) => {
    try {
      // En una implementación real, usarías el credentialResponse.credential para autenticarte
      // con tu backend y obtener un token JWT
      console.log('Registro con Google exitoso:', credentialResponse);
      // Aquí normalmente llamarías a una función para procesar el registro de Google
      // await registerWithGoogle(credentialResponse.credential);
    } catch (error) {
      console.error('Error en el registro con Google:', error);
      setGeneralError('Error en el registro con Google');
    }
  };

  const handleGoogleError = () => {
    console.error('Registro con Google fallido');
    setGeneralError('Registro con Google fallido');
  };

  return (
    <div className="register-form-container">
      <h2>Crear Cuenta</h2>
      
      {generalError && (
        <div className="error-message">
          {generalError}
        </div>
      )}
      
      <form onSubmit={handleSubmit} className="register-form">
        <div className="form-group">
          <label htmlFor="nombre">Nombre</label>
          <input
            type="text"
            id="nombre"
            name="nombre"
            value={formData.nombre}
            onChange={handleChange}
            className={errors.nombre ? 'error' : ''}
          />
          {errors.nombre && <span className="error-text">{errors.nombre}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="apellido">Apellido</label>
          <input
            type="text"
            id="apellido"
            name="apellido"
            value={formData.apellido}
            onChange={handleChange}
            className={errors.apellido ? 'error' : ''}
          />
          {errors.apellido && <span className="error-text">{errors.apellido}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="email">Correo Electrónico</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className={errors.email ? 'error' : ''}
          />
          {errors.email && <span className="error-text">{errors.email}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="password">Contraseña</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            className={errors.password ? 'error' : ''}
          />
          {errors.password && <span className="error-text">{errors.password}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="confirmPassword">Confirmar Contraseña</label>
          <input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
            className={errors.confirmPassword ? 'error' : ''}
          />
          {errors.confirmPassword && <span className="error-text">{errors.confirmPassword}</span>}
        </div>
        
        <button type="submit" disabled={loading} className="btn-register">
          {loading ? 'Registrando...' : 'Registrarse'}
        </button>
      </form>
      
      {/* Botón de Google Login */}
      <div className="google-login-section">
        <p>O regístrate con:</p>
        <GoogleLogin
          onSuccess={handleGoogleSuccess}
          onError={handleGoogleError}
          useOneTap
        />
      </div>
      
      <p className="switch-auth">
        ¿Ya tienes cuenta? <button onClick={onSwitchToLogin} className="link-button">Iniciar sesión</button>
      </p>
    </div>
  );
};

export default RegisterForm;