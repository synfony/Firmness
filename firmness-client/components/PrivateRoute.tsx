'use client';

import { ReactNode, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/context/AuthContext';
import LoadingSpinner from './LoadingSpinner'; // Importar el nuevo componente

interface PrivateRouteProps {
  children: ReactNode;
}

const PrivateRoute = ({ children }: PrivateRouteProps) => {
  const { isAuthenticated, loading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!loading && !isAuthenticated) {
      router.push('/auth/login');
    }
  }, [isAuthenticated, loading, router]);

  if (loading) {
    // Muestra el spinner de carga mientras se verifica la sesión
    return <LoadingSpinner />;
  }

  if (!isAuthenticated) {
    // No renderiza nada mientras redirige para evitar parpadeos
    return null;
  }

  // Si está autenticado, muestra el contenido de la página
  return <>{children}</>;
};

export default PrivateRoute;
