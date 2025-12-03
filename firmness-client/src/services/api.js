import axios from 'axios';

// Create an Axios instance with the base configuration
const apiClient = axios.create({
  // Use environment variable for the API base URL
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Interceptor to add the JWT to every request
apiClient.interceptors.request.use(
  config => {
    const token = localStorage.getItem('token'); // Get token from LocalStorage
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

export default apiClient;