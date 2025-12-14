import React, { useState } from 'react';
import { validateRegisterForm, validateEmail, validatePassword, validateNombre, validateApellido } from '../../utils/validators';
import { PASSWORD_REQUIREMENTS } from '../../utils/constants';
import './RegisterForm.css';

interface RegisterFormData {
  nombre: string;
  apellido: string;
  email: string;
  password: string;
  confirmPassword: string;
}

const RegisterForm: React.FC<{ onRegister: (userData: RegisterFormData) => void }> = ({ onRegister }) => {
  const [formData, setFormData] = useState<RegisterFormData>({
    nombre: '',
    apellido: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const [errors, setErrors] = useState<Record<string, string>>({});
  const [showPasswordRequirements, setShowPasswordRequirements] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
    
    // Limpiar error cuando el usuario escribe
    if (errors[name]) {
      setErrors(prev => {
        const newErrors = { ...prev };
        delete newErrors[name];
        return newErrors;
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const validation = validateRegisterForm(formData);
    
    if (!validation.isValid) {
      setErrors(validation.errors);
      return;
    }

    setIsSubmitting(true);
    try {
      await onRegister(formData);
    } catch (error) {
      console.error('Error en el registro:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleGoogleLogin = () => {
    // Simulación de inicio de sesión con Google
    alert('Inicio de sesión con Google no implementado aún');
  };

  return (
    <div className="register-form-container">
      <div className="register-form-card">
        <h2>Crear Cuenta</h2>
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="nombre">Nombre *</label>
            <input
              type="text"
              id="nombre"
              name="nombre"
              value={formData.nombre}
              onChange={handleChange}
              className={errors.nombre ? 'error' : ''}
              placeholder="Ingresa tu nombre"
            />
            {errors.nombre && <span className="error-message">{errors.nombre}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="apellido">Apellido *</label>
            <input
              type="text"
              id="apellido"
              name="apellido"
              value={formData.apellido}
              onChange={handleChange}
              className={errors.apellido ? 'error' : ''}
              placeholder="Ingresa tu apellido"
            />
            {errors.apellido && <span className="error-message">{errors.apellido}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="email">Correo Electrónico *</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              className={errors.email ? 'error' : ''}
              placeholder="ejemplo@correo.com"
            />
            {errors.email && <span className="error-message">{errors.email}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="password">Contraseña *</label>
            <div className="password-input-container">
              <input
                type="password"
                id="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                onFocus={() => setShowPasswordRequirements(true)}
                onBlur={() => setShowPasswordRequirements(false)}
                className={errors.password ? 'error' : ''}
                placeholder="Crea una contraseña segura"
              />
              {errors.password && <span className="error-message">{errors.password}</span>}
            </div>
            
            {showPasswordRequirements && (
              <div className="password-requirements">
                <p>Requisitos de contraseña:</p>
                <ul>
                  {PASSWORD_REQUIREMENTS.map((req, index) => (
                    <li 
                      key={index} 
                      className={
                        req.includes('8 caracteres') ? (formData.password.length >= 8 ? 'valid' : 'invalid') :
                        req.includes('mayúscula') ? (/[A-Z]/.test(formData.password) ? 'valid' : 'invalid') :
                        req.includes('minúscula') ? (/[a-z]/.test(formData.password) ? 'valid' : 'invalid') :
                        req.includes('número') ? (/\d/.test(formData.password) ? 'valid' : 'invalid') :
                        req.includes('especial') ? (/[@$!%*?&]/.test(formData.password) ? 'valid' : 'invalid') :
                        'neutral'
                      }
                    >
                      {req}
                    </li>
                  ))}
                </ul>
              </div>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="confirmPassword">Confirmar Contraseña *</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleChange}
              className={errors.confirmPassword ? 'error' : ''}
              placeholder="Confirma tu contraseña"
            />
            {errors.confirmPassword && <span className="error-message">{errors.confirmPassword}</span>}
          </div>

          <button 
            type="submit" 
            className="register-button"
            disabled={isSubmitting}
          >
            {isSubmitting ? 'Registrando...' : 'Registrarse'}
          </button>
        </form>

        <div className="divider">
          <span>O continúa con</span>
        </div>

        <button 
          className="google-login-button"
          onClick={handleGoogleLogin}
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24">
            <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
            <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
            <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
            <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
          </svg>
          Registrarse con Google
        </button>

        <p className="terms-text">
          Al registrarte, aceptas nuestros Términos de Servicio y Política de Privacidad.
        </p>
      </div>
    </div>
  );
};

export default RegisterForm;