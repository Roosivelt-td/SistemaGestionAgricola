import React from 'react';
import { Routes as RouterRoutes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import { useAuth } from './hooks/useAuth';

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" />;
};

const Routes = () => {
  return (
    <RouterRoutes>
      <Route path="/login" element={<LoginPage />} />
      <Route 
        path="/" 
        element={
          <PrivateRoute>
            <DashboardPage />
          </PrivateRoute>
        } 
      />
      {/* Agregar más rutas aquí */}
    </RouterRoutes>
  );
};

export default Routes;