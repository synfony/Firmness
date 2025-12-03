import apiClient from './api';

export default {
  createSale(saleData) {
    // Llama al endpoint POST /api/sales para crear una nueva venta
    return apiClient.post('/sales', saleData);
  }
};