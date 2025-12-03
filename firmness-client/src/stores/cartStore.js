import { defineStore } from 'pinia';
import salesService from '../services/salesService';

export const useCartStore = defineStore('cart', {
  state: () => ({
    items: [], // Each item will be { product, quantity }
  }),
  getters: {
    cartItemCount: (state) => {
      return state.items.reduce((total, item) => total + item.quantity, 0);
    },
    cartTotal: (state) => {
      return state.items.reduce((total, item) => total + (item.product.price * item.quantity), 0).toFixed(2);
    },
  },
  actions: {
    addToCart(product) {
      const existingItem = this.items.find(item => item.product.id === product.id);

      if (existingItem) {
        existingItem.quantity++;
      } else {
        this.items.push({ product: product, quantity: 1 });
      }
    },
    removeFromCart(productId) {
      this.items = this.items.filter(item => item.product.id !== productId);
    },
    clearCart() {
      this.items = [];
    },
    async checkout() {
      try {
        // Format data for the API
        const saleData = {
          saleDetails: this.items.map(item => ({
            productId: item.product.id,
            quantity: item.quantity,
            unitPrice: item.product.price
          }))
        };

        await salesService.createSale(saleData);

        // Clear the cart
        this.clearCart();
        
        // Optional: Redirect or show success message
        alert('Purchase successful! A confirmation has been sent to your email.');

      } catch (error) {
        console.error('Error during checkout:', error);
        alert('There was an error with your purchase. Please try again.');
      }
    }
  },
});