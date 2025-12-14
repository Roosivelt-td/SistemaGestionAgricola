import React from 'react';
import { useAuth } from '../hooks/useAuth';

const DashboardPage: React.FC = () => {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <div className="dashboard-container">
      <header>
        <h1>Panel de Control - Agricultura</h1>
        <div className="user-info">
          <span>Bienvenido, {user?.nombre} {user?.apellido}</span>
          <button onClick={handleLogout} className="logout-btn">Cerrar Sesión</button>
        </div>
      </header>
      
      <main>
        <div className="dashboard-content">
          <h2>Resumen del Sistema</h2>
          <p>Esta es la página principal del sistema de gestión agrícola.</p>
          
          <div className="stats-grid">
            <div className="stat-card">
              <h3>Cultivos Activos</h3>
              <p>12</p>
            </div>
            <div className="stat-card">
              <h3>Terrenos Registrados</h3>
              <p>5</p>
            </div>
            <div className="stat-card">
              <h3>Próximas Cosechas</h3>
              <p>3</p>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default DashboardPage;