// Validadores de campos de formulario
export const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

export const validatePassword = (password: string): boolean => {
  // Al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial
  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
  return passwordRegex.test(password);
};

export const validateNombre = (nombre: string): boolean => {
  // Solo letras y espacios, al menos 2 caracteres
  const nombreRegex = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{2,}$/;
  return nombreRegex.test(nombre);
};

export const validateApellido = (apellido: string): boolean => {
  // Solo letras y espacios, al menos 2 caracteres
  const apellidoRegex = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{2,}$/;
  return apellidoRegex.test(apellido);
};

// Validador de formulario de registro
export const validateRegisterForm = (formData: {
  nombre: string;
  apellido: string;
  email: string;
  password: string;
  confirmPassword: string;
}): { isValid: boolean; errors: Record<string, string> } => {
  const errors: Record<string, string> = {};
  
  if (!formData.nombre.trim()) {
    errors.nombre = 'El nombre es obligatorio';
  } else if (!validateNombre(formData.nombre)) {
    errors.nombre = 'El nombre solo puede contener letras y espacios';
  }
  
  if (!formData.apellido.trim()) {
    errors.apellido = 'El apellido es obligatorio';
  } else if (!validateApellido(formData.apellido)) {
    errors.apellido = 'El apellido solo puede contener letras y espacios';
  }
  
  if (!formData.email.trim()) {
    errors.email = 'El correo electrónico es obligatorio';
  } else if (!validateEmail(formData.email)) {
    errors.email = 'Formato de correo electrónico inválido';
  }
  
  if (!formData.password) {
    errors.password = 'La contraseña es obligatoria';
  } else if (!validatePassword(formData.password)) {
    errors.password = 'La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial';
  }
  
  if (!formData.confirmPassword) {
    errors.confirmPassword = 'Confirma tu contraseña';
  } else if (formData.password !== formData.confirmPassword) {
    errors.confirmPassword = 'Las contraseñas no coinciden';
  }
  
  return {
    isValid: Object.keys(errors).length === 0,
    errors
  };
};