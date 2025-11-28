'use client';

import { useState } from 'react';
import { useAuth } from '@/context/AuthContext';
import { LoginViewModel } from '@/types';
import Link from 'next/link';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const data: LoginViewModel = { Email: email, Password: password };
    await login(data);
  };

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      {/* Header simple y limpio */}
      <header className="absolute top-0 left-0 w-full p-4">
        <h1 className="text-xl font-bold text-gray-800">Firmeza</h1>
      </header>

      {/* Contenedor principal centrado */}
      <main className="flex flex-1 justify-center items-center">
        <div className="w-full max-w-md px-4">
          {/* Formulario con sombra y bordes redondeados */}
          <form
            onSubmit={handleSubmit}
            className="bg-white p-8 rounded-lg shadow-md space-y-6"
          >
            <div className="text-center">
              <h2 className="text-2xl font-bold text-gray-900">Iniciar Sesión</h2>
              <p className="text-gray-500 mt-2">Accede a tu cuenta</p>
            </div>

            {/* Campos del formulario */}
            <div className="space-y-4">
              <div>
                <label
                  htmlFor="email"
                  className="block text-sm font-medium text-gray-700 mb-1"
                >
                  Correo Electrónico
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  required
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                  placeholder="tu@email.com"
                />
              </div>
              <div>
                <label
                  htmlFor="password"
                  className="block text-sm font-medium text-gray-700 mb-1"
                >
                  Contraseña
                </label>
                <input
                  id="password"
                  name="password"
                  type="password"
                  required
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                  placeholder="••••••••"
                />
              </div>
            </div>

            {/* Botón de envío */}
            <div>
              <button
                type="submit"
                className="w-full px-4 py-3 font-semibold text-white bg-blue-600 rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors"
              >
                Iniciar Sesión
              </button>
            </div>
          </form>

          {/* Enlace de registro */}
          <p className="text-center text-sm text-gray-600 mt-6">
            ¿No tienes una cuenta?{' '}
            <Link href="/auth/register" className="font-medium text-blue-600 hover:underline">
              Regístrate aquí
            </Link>
          </p>
        </div>
      </main>
    </div>
  );
}
