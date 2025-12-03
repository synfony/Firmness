<script setup>
import { useCartStore } from '@/stores/cartStore';
import { storeToRefs } from 'pinia';

const cartStore = useCartStore();
const { items, cartTotal } = storeToRefs(cartStore);
</script>

<template>
  <div class="cart-view">
    <h1>Shopping Cart</h1>
    
    <div v-if="items.length === 0">
      <p>Your cart is empty.</p>
    </div>
    
    <div v-else>
      <div class="cart-items">
        <div v-for="item in items" :key="item.product.id" class="cart-item">
          <span>{{ item.product.name }}</span>
          <span>{{ item.quantity }} x ${{ item.product.price }}</span>
          <button @click="cartStore.removeFromCart(item.product.id)">Remove</button>
        </div>
      </div>
      
      <div class="cart-total">
        <h3>Total: ${{ cartTotal }}</h3>
        <button @click="cartStore.checkout()">Proceed to Checkout</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.cart-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
  border-bottom: 1px solid #eee;
}

.cart-total {
  margin-top: 1rem;
  text-align: right;
}
</style>