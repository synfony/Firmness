'use client';

import { useEffect, useState } from 'react';
import apiClient from '@/lib/api';
import { ProductDto } from '@/types';
import ProductCard from '@/components/ProductCard';
import PrivateRoute from '@/components/PrivateRoute';

function ProductsPageContent() {
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await apiClient.get('/api/products');
        setProducts(response.data);
      } catch (error) {
        console.error('Error fetching products:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchProducts();
  }, []);

  if (loading) {
    return <div className="flex items-center justify-center min-h-screen">Cargando productos...</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-6">Catálogo de Productos</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </div>
  );
}

// Envolver el contenido con el guardián de ruta
export default function ProductsPage() {
  return (
    <PrivateRoute>
      <ProductsPageContent />
    </PrivateRoute>
  );
}
