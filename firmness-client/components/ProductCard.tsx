'use client';

import { ProductDto } from '@/types';
import { useCart } from '@/context/CartContext';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';

interface ProductCardProps {
  product: ProductDto;
}

const ProductCard = ({ product }: ProductCardProps) => {
  const { addToCart } = useCart();
  const { isAuthenticated } = useAuth();
  const router = useRouter();

  const handleAddToCart = () => {
    if (isAuthenticated) {
      addToCart(product);
    } else {
      // Si no está autenticado, redirige al login
      router.push('/auth/login');
    }
  };

  return (
    <div className="bg-white rounded-xl shadow-lg overflow-hidden flex flex-col group transition-all duration-300 hover:shadow-2xl">
      <div className="bg-gray-200 h-48 flex items-center justify-center">
        <span className="text-gray-400">Imagen no disponible</span>
      </div>
      <div className="p-4 flex flex-col flex-grow">
        <h2 className="text-lg font-semibold text-gray-800 truncate">{product.name}</h2>
        <p className="text-gray-500 text-sm mt-1 flex-grow h-12">{product.description}</p>
        <div className="mt-4 flex items-center justify-between">
          <span className="text-xl font-bold text-gray-900">${product.price.toFixed(2)}</span>
          <button
            onClick={handleAddToCart}
            className="px-4 py-2 flex items-center space-x-2 font-semibold text-white bg-blue-600 rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transform transition-transform duration-200 group-hover:scale-105"
          >
            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
              <path d="M3 1a1 1 0 000 2h1.22l.305 1.222a.997.997 0 00.922.778h9.906a1 1 0 00.922-.778L17.78 3H19a1 1 0 100-2H3zM6.22 8.222a1 1 0 00-.922 1.222l1.5 6A1 1 0 008 16h9a1 1 0 00.922-.778l1.5-6a1 1 0 00-.922-1.222H6.22z" />
            </svg>
            <span>Añadir</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
