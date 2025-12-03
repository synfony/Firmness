import { defineStore } from 'pinia';
import authService from '../services/authService';
import router from '../router';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    user: null // We could store user info here if the API returns it
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    async login(credentials) {
      try {
        const response = await authService.login(credentials);
        const token = response.data.token;

        // Save the token
        this.token = token;
        localStorage.setItem('token', token);

        // Redirect the user
        router.push('/'); // Redirect to the home page
      } catch (error) {
        console.error('Login Error:', error);
        alert('Login failed. Please check your credentials and try again.');
      }
    },
    logout() {
      // Clear the state
      this.token = null;
      this.user = null;
      localStorage.removeItem('token');

      // Redirect to login
      router.push('/login');
    },
    async register(userInfo) {
      try {
        await authService.register(userInfo);
        // Optional: Automatically log in after registration
        await this.login({ email: userInfo.email, password: userInfo.password });
      } catch (error) {
        console.error('Registration Error:', error);
        alert('Registration failed. The user might already exist.');
      }
    }
  }
});