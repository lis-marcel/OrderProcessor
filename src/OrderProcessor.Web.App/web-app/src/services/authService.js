// src/services/authService.js
import axios from 'axios'

const API_URL = 'https://127.0.0.1:7092/api/customer/'

export default {
  async login(email, password) {
    const response = await axios.post(API_URL + 'login', { email, password })
    if (response.data.sessionToken) {
      // Store the session token
      localStorage.setItem('token', response.data.sessionToken)
      // Store user data from customerData
      localStorage.setItem('user', JSON.stringify(response.data.customerData))
    }
    return response.data
  },
  
  async register(name, email, password) {
    const response = await axios.post(API_URL + 'register', { name, email, password })
    if (response.data.sessionToken) {
      // Store the session token
      localStorage.setItem('token', response.data.sessionToken)
      // Store user data from customerData
      localStorage.setItem('user', JSON.stringify(response.data.customerData))
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