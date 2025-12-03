<script setup>
import { ref } from 'vue';
import { RouterLink } from 'vue-router'; // Import RouterLink
import { useAuthStore } from '@/stores/authStore';

const authStore = useAuthStore();

const email = ref('');
const password = ref('');

const handleLogin = async () => {
  await authStore.login({
    email: email.value,
    password: password.value
  });
};
</script>

<template>
  <div class="login-view">
    <form @submit.prevent="handleLogin">
      <h1>Login</h1>
      <div class="form-group">
        <label for="email">Email</label>
        <input type="email" id="email" v-model="email" required />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="password" required />
      </div>
      <button type="submit">Login</button>
      <p class="register-link">
        Don't have an account? <RouterLink to="/register">Register here</RouterLink>
      </p>
    </form>
  </div>
</template>

<style scoped>
.login-view {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 80vh;
}

form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 2rem;
  border: 1px solid #ccc;
  border-radius: 8px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.register-link {
  text-align: center;
  margin-top: 1rem;
}
</style>