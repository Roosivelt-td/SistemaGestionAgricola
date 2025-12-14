import React, { useState } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { validateEmail } from '../../utils/validators';
import { GoogleLogin } from 'react-google-login';

interface FormData {
  email: string;
  password: string;
}

const LoginForm: React.FC<{ onSwitchToRegister: () => void }> = ({ onSwitchToRegister }) => {
  const { login, loading } = useAuth();
  const [formData, setFormData] = useState<FormData>({
    email: '',
    password: ''
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
  };

  const validateForm = (): boolean => {
    const newErrors: Partial<FormData> = {};

    if (!formData.email.trim()) {
      newErrors.email = 'El correo electrónico es obligatorio';
    } else if (!validateEmail(formData.email)) {
      newErrors.email = 'Por favor ingrese un correo electrónico válido';
    }

    if (!formData.password.trim()) {
      newErrors.password = 'La contraseña es obligatoria';
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
      await login(formData.email, formData.password);
    } catch (error: any) {
      setGeneralError(error.message || 'Error en el inicio de sesión');
    }
  };

  // Configuración para login con Google
  const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID || 'YOUR_GOOGLE_CLIENT_ID';

  const handleGoogleSuccess = async (response: any) => {
    try {
      // En una implementación real, usarías el token de Google para autenticarte
      // con tu backend y obtener un token JWT
      console.log('Inicio de sesión con Google exitoso:', response);
      // Aquí normalmente llamarías a una función para procesar el login de Google
    } catch (error) {
      console.error('Error en el inicio de sesión con Google:', error);
      setGeneralError('Error en el inicio de sesión con Google');
    }
  };

  const handleGoogleFailure = (error: any) => {
    console.error('Inicio de sesión con Google fallido:', error);
    setGeneralError('Inicio de sesión con Google fallido');
  };

  return (
    <div className="login-form-container">
      <h2>Iniciar Sesión</h2>
      
      {generalError && (
        <div className="error-message">
          {generalError}
        </div>
      )}
      
      <form onSubmit={handleSubmit} className="login-form">
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
        
        <button type="submit" disabled={loading} className="btn-login">
          {loading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
        </button>
      </form>
      
      {/* Botón de Google Login */}
      <div className="google-login-section">
        <p>O inicia sesión con:</p>
        <GoogleLogin
          clientId={clientId}
          buttonText="Iniciar sesión con Google"
          onSuccess={handleGoogleSuccess}
          onFailure={handleGoogleFailure}
          cookiePolicy={'single_host_origin'}
          className="google-login-btn"
        />
      </div>
      
      <p className="switch-auth">
        ¿No tienes cuenta? <button onClick={onSwitchToRegister} className="link-button">Regístrate</button>
      </p>
    </div>
  );
};

export default LoginForm;