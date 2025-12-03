import apiClient from './api';

export default {
  login(credentials) {
    return apiClient.post('/Auth/login', credentials);
  },
  register(userInfo) {
    return apiClient.post('/Auth/register', userInfo);
  }
};