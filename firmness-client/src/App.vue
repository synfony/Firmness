<script setup>
import { RouterLink, RouterView } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { useCartStore } from '@/stores/cartStore'; // Importar la tienda del carrito

const authStore = useAuthStore();
const cartStore = useCartStore();
</script>

<template>
  <header>
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/cart" v-if="authStore.isAuthenticated">
        Cart ({{ cartStore.cartItemCount }})
      </RouterLink>
      <div class="spacer"></div>
      <RouterLink to="/login" v-if="!authStore.isAuthenticated">Login</RouterLink>
      <button @click="authStore.logout()" v-if="authStore.isAuthenticated">Logout</button>
    </nav>
  </header>

  <RouterView />
</template>

<style scoped>
nav {
  display: flex;
  gap: 1rem;
  padding: 1rem;
  background-color: #f0f0f0;
  align-items: center;
}

.spacer {
  flex-grow: 1;
}
</style>