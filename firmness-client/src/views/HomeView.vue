<script setup>
import { onMounted } from 'vue';
import { useProductsStore } from '@/stores/productsStore';
import { useCartStore } from '@/stores/cartStore'; // Importar la tienda del carrito
import { storeToRefs } from 'pinia';

const productsStore = useProductsStore();
const { products, isLoading, error } = storeToRefs(productsStore);

const cartStore = useCartStore(); // Usar la tienda del carrito

// Obtener los productos cuando el componente se monta
onMounted(() => {
  productsStore.fetchProducts();
});
</script>

<template>
  <main>
    <h1>Product Catalog</h1>
    
    <div v-if="isLoading">
      <p>Loading products...</p>
    </div>
    
    <div v-else-if="error">
      <p style="color: red;">{{ error }}</p>
    </div>
    
    <div v-else-if="products.length > 0" class="product-grid">
      <div v-for="product in products" :key="product.id" class="product-card">
        <h2>{{ product.name }}</h2>
        <p>{{ product.description }}</p>
        <p><strong>Price:</strong> ${{ product.price }}</p>
        <button @click="cartStore.addToCart(product)">Add to Cart</button>
      </div>
    </div>
    
    <div v-else>
      <p>No products found.</p>
    </div>
  </main>
</template>

<style scoped>
.product-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
}

.product-card {
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 1rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

button {
  margin-top: 1rem;
}
</style>