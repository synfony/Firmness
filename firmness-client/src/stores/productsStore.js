import { defineStore } from 'pinia';
import productsService from '../services/productsService';

export const useProductsStore = defineStore('products', {
  state: () => ({
    products: [],
    isLoading: false,
    error: null
  }),
  actions: {
    async fetchProducts() {
      this.isLoading = true;
      this.error = null;
      try {
        const response = await productsService.getProducts();
        this.products = response.data;
      } catch (error) {
        console.error('Error fetching products:', error);
        this.error = 'Could not fetch products.';
      } finally {
        this.isLoading = false;
      }
    }
  }
});