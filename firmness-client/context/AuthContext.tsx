'use client';

import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { useRouter } from 'next/navigation';
import apiClient from '@/lib/api';
import { LoginViewModel, RegisterViewModel } from '@/types';

interface AuthContextType {
  isAuthenticated: boolean;
  user: any;
  loading: boolean;
  login: (data: LoginViewModel) => Promise<void>;
  register: (data: RegisterViewModel) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    try {
      const token = localStorage.getItem('token');
      if (token) {
        setUser({ token });
      }
    } catch (error) {
      console.error('Failed to initialize auth state:', error);
    } finally {
      setLoading(false);
    }
  }, []);

  const login = async (data: LoginViewModel) => {
    try {
      const response = await apiClient.post('/api/auth/login', data);
      const { token } = response.data;
      
      localStorage.setItem('token', token);
      apiClient.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      setUser({ token });
      
      // Redirigir al catálogo de productos
      router.push('/products');
    } catch (error) {
      console.error('Login failed:', error);
    }
  };

  const register = async (data: RegisterViewModel) => {
    try {
      await apiClient.post('/api/auth/register', data);
      await login({ Email: data.Email, Password: data.Password, RememberMe: false });
    } catch (error) {
      console.error('Registration failed:', error);
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    delete apiClient.defaults.headers.common['Authorization'];
    setUser(null);
    router.push('/auth/login');
  };

  const isAuthenticated = !!user;

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, loading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
