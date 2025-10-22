import React, { lazy, Suspense, useEffect, useState } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { Spin } from 'antd';
import { useAuthStore } from '../../store/authStore';
import { Protected } from '../../features/auth/components/ProtectedRoutes';
import PublicRoutes from '../../features/auth/components/PublicRoutes';

const Home = lazy(() => import('../../pages/Home'));
const Login = lazy(() => import('../../pages/Login'));

export const AppRoutes: React.FC = () => {
  const { user, fetchMe } = useAuthStore();
  const [isFirstTime, setIsFirstTime] = useState(true);
  const isAuthenticated = !!user;

  useEffect(() => {
    const verifyAuth = async () => {
      try {
        await fetchMe();
      }
      finally {
        setIsFirstTime(false);
      }
    };

    verifyAuth();
  }, [fetchMe]);

  const renderLoading = () => {
    return <div className='loading-container'>
      <Spin percent='auto' size="large" />
    </div>
  }

  if (isFirstTime) {
    return renderLoading();
  }

  return <Suspense fallback={<div style={{ padding: 24 }}>Loading...</div>}>
    <Routes>
      <Route element={<PublicRoutes />}>
        <Route path="/login" element={<Login />} />
      </Route>

      <Route element={<Protected />}>
        <Route index element={<Home />} />
      </Route>

      <Route path="*" element={<Navigate to={isAuthenticated ? "/" : '/login'} replace />} />
    </Routes>
  </Suspense>
};