import { useAuth as useAuthContext } from '../contexts/AuthContext';

// Este hook simplemente envuelve el contexto para facilitar su uso
// y permitir futuras extensiones si es necesario
export const useAuth = () => {
  const context = useAuthContext();
  
  if (!context) {
    throw new Error('useAuth debe usarse dentro de un AuthProvider');
  }
  
  return context;
};