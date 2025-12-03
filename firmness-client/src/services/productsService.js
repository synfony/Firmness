import apiClient from './api';

export default {
  getProducts() {
    // Llama al endpoint GET /api/products que creamos en la API
    return apiClient.get('/products');
  }
};