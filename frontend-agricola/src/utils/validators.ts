// Validadores para formularios
export const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

export const validatePassword = (password: string): boolean => {
  // Al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial
  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
  return passwordRegex.test(password);
};

export const validateName = (name: string): boolean => {
  // Solo letras y espacios, mínimo 2 caracteres
  const nameRegex = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{2,}$/;
  return nameRegex.test(name);
};

export const validateRequired = (value: string): boolean => {
  return value.trim().length > 0;
};

// Mensajes de error
export const getErrorMessage = (field: string, value: string, validationType?: string): string => {
  switch(field) {
    case 'email':
      if (!validateRequired(value)) {
        return 'El correo electrónico es obligatorio';
      }
      if (!validateEmail(value)) {
        return 'Por favor ingrese un correo electrónico válido';
      }
      return '';
    
    case 'password':
      if (!validateRequired(value)) {
        return 'La contraseña es obligatoria';
      }
      if (!validatePassword(value)) {
        return 'La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial';
      }
      return '';
    
    case 'nombre':
    case 'apellido':
      if (!validateRequired(value)) {
        return 'Este campo es obligatorio';
      }
      if (!validateName(value)) {
        return 'Por favor ingrese un nombre válido (solo letras)';
      }
      return '';
    
    default:
      return '';
  }
};