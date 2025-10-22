import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuthStore } from '../../../store/authStore';

const PublicRoutes: React.FC = () => {
  const { user } = useAuthStore();

  if (user) {
    return <Navigate to='/' replace />;
  }

  return <Outlet />;
};

export default PublicRoutes;