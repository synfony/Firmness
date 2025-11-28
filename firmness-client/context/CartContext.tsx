'use client';

import { createContext, useContext, useState, ReactNode } from 'react';
import { CartItem, ProductDto, SaleDto, SaleDetailDto } from '@/types';
import apiClient from '@/lib/api';

// Definir la forma del contexto del carrito
interface CartContextType {
  cartItems: CartItem[];
  addToCart: (product: ProductDto) => void;
  removeFromCart: (productId: number) => void;
  updateQuantity: (productId: number, quantity: number) => void;
  clearCart: () => void;
  checkout: () => Promise<boolean>; // Devuelve true si la compra fue exitosa
  cartTotal: number;
}

// Crear el contexto con un valor por defecto
const CartContext = createContext<CartContextType | undefined>(undefined);

// Crear el proveedor del contexto
export const CartProvider = ({ children }: { children: ReactNode }) => {
  const [cartItems, setCartItems] = useState<CartItem[]>([]);

  const addToCart = (product: ProductDto) => {
    setCartItems((prevItems) => {
      const existingItem = prevItems.find((item) => item.id === product.id);
      if (existingItem) {
        return prevItems.map((item) =>
          item.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
        );
      }
      return [...prevItems, { ...product, quantity: 1 }];
    });
  };

  const removeFromCart = (productId: number) => {
    setCartItems((prevItems) => prevItems.filter((item) => item.id !== productId));
  };

  const updateQuantity = (productId: number, quantity: number) => {
    if (quantity <= 0) {
      removeFromCart(productId);
    } else {
      setCartItems((prevItems) =>
        prevItems.map((item) =>
          item.id === productId ? { ...item, quantity } : item
        )
      );
    }
  };

  const clearCart = () => {
    setCartItems([]);
  };

  const checkout = async (): Promise<boolean> => {
    if (cartItems.length === 0) {
      alert('Tu carrito está vacío.');
      return false;
    }

    // TODO: Obtener el ID del cliente real desde el AuthContext
    const clientId = 1; // Simplificación temporal

    const saleDetails: SaleDetailDto[] = cartItems.map(item => ({
      productId: item.id,
      quantity: item.quantity,
      unitPrice: item.price,
    }));

    const sale: SaleDto = {
      clientId,
      saleDetails,
    };

    try {
      await apiClient.post('/api/sales', sale);
      clearCart(); // Limpiar el carrito después de una compra exitosa
      return true;
    } catch (error) {
      console.error('Error during checkout:', error);
      alert('Hubo un error al procesar tu compra. Por favor, inténtalo de nuevo.');
      return false;
    }
  };

  const cartTotal = cartItems.reduce(
    (total, item) => total + item.price * item.quantity,
    0
  );

  return (
    <CartContext.Provider
      value={{ cartItems, addToCart, removeFromCart, updateQuantity, clearCart, checkout, cartTotal }}
    >
      {children}
    </CartContext.Provider>
  );
};

// Hook personalizado para usar el contexto del carrito
export const useCart = () => {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};
