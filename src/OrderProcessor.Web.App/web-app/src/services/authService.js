// src/services/authService.js
import axios from 'axios'

const API_URL = 'https://127.0.0.1:7092/api/auth/'

export default {
  async login(email, password) {
    const response = await axios.post(API_URL + 'login', { email, password })
    if (response.data.token) {
      localStorage.setItem('token', response.data.token)
      localStorage.setItem('user', JSON.stringify(response.data.user))
    }
    return response.data
  },
  
  async register(name, email, password) {
    const response = await axios.post(API_URL + 'register', { name, email, password })
    if (response.data.token) {
      localStorage.setItem('token', response.data.token)
      localStorage.setItem('user', JSON.stringify(response.data.user))
    }
    return response.data
  },
  
  logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  },
  
  getCurrentUser() {
    return JSON.parse(localStorage.getItem('user'))
  },
  
  isAuthenticated() {
    return !!localStorage.getItem('token')
  }
}