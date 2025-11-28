'use client';

import { useState } from 'react';
import { useCart } from '@/context/CartContext';
import Link from 'next/link';
import PrivateRoute from '@/components/PrivateRoute'; // Importar el guardián

export default function CartPage() {
  const { cartItems, updateQuantity, removeFromCart, cartTotal, checkout } = useCart();
  const [isCheckingOut, setIsCheckingOut] = useState(false);
  const [purchaseSuccess, setPurchaseSuccess] = useState(false);

  const ivaRate = 0.19;
  const iva = cartTotal * ivaRate;
  const grandTotal = cartTotal + iva;

  const handleCheckout = async () => {
    setIsCheckingOut(true);
    const success = await checkout();
    if (success) {
      setPurchaseSuccess(true);
    }
    setIsCheckingOut(false);
  };

  if (purchaseSuccess) {
    return (
      <div className="container mx-auto px-4 py-8 text-center">
        <h1 className="text-3xl font-bold text-green-600 mb-4">¡Compra Exitosa!</h1>
        <p className="text-lg mb-6">Hemos procesado tu pedido correctamente. Se ha enviado un comprobante a tu correo electrónico.</p>
        <Link href="/" className="text-blue-600 hover:underline">
          Volver al catálogo
        </Link>
      </div>
    );
  }

  return (
    <PrivateRoute>
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-3xl font-bold mb-8 text-gray-800">Tu Carrito</h1>
        {cartItems.length === 0 ? (
          <div className="text-center bg-white p-10 rounded-lg shadow-md">
            <p className="text-xl text-gray-600 mb-4">Tu carrito está vacío.</p>
            <Link href="/" className="inline-block px-6 py-3 font-semibold text-white bg-blue-600 rounded-lg hover:bg-blue-700 transition-colors">
              Ir al Catálogo
            </Link>
          </div>
        ) : (
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div className="lg:col-span-2 bg-white p-6 rounded-lg shadow-md">
              <h2 className="text-xl font-bold mb-4 border-b pb-4">Artículos</h2>
              <div className="space-y-4">
                {cartItems.map((item) => (
                  <div key={item.id} className="flex items-center justify-between">
                    <div>
                      <h3 className="font-semibold">{item.name}</h3>
                      <p className="text-sm text-gray-500">${item.price.toFixed(2)}</p>
                    </div>
                    <div className="flex items-center space-x-4">
                      <input
                        type="number"
                        min="1"
                        value={item.quantity}
                        onChange={(e) => updateQuantity(item.id, parseInt(e.target.value, 10))}
                        className="w-20 px-2 py-1 border rounded-md text-center"
                      />
                      <button onClick={() => removeFromCart(item.id)} className="text-red-500 hover:text-red-700 transition-colors">
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                        </svg>
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
            <div className="bg-gray-50 p-6 rounded-lg shadow-md h-fit">
              <h2 className="text-xl font-bold mb-4">Resumen</h2>
              <div className="space-y-3">
                <div className="flex justify-between">
                  <span>Subtotal</span>
                  <span>${cartTotal.toFixed(2)}</span>
                </div>
                <div className="flex justify-between">
                  <span>IVA (19%)</span>
                  <span>${iva.toFixed(2)}</span>
                </div>
                <div className="flex justify-between font-bold text-lg border-t pt-3 mt-3">
                  <span>Total</span>
                  <span>${grandTotal.toFixed(2)}</span>
                </div>
              </div>
              <button
                onClick={handleCheckout}
                disabled={isCheckingOut}
                className="w-full mt-6 px-4 py-3 font-semibold text-white bg-green-600 rounded-lg hover:bg-green-700 disabled:bg-gray-400 transition-colors"
              >
                {isCheckingOut ? 'Procesando...' : 'Finalizar Compra'}
              </button>
            </div>
          </div>
        )}
      </div>
    </PrivateRoute>
  );
}
