// src/services/authService.js
import axios from 'axios'

const API_URL = 'https://127.0.0.1:7092/api/customer/'

export default {
  async login(email, password) {
    const response = await axios.post(API_URL + 'login', { email, password })
    if (response.data.token) {
      // Store the session token
      localStorage.setItem('token', response.data.token)
      // Store user data from customerData
      localStorage.setItem('user', JSON.stringify(response.data.customerData))
    }
    return response.data
  },
  
  async register(name, email, password) {
    const response = await axios.post(API_URL + 'register', { name, email, password })
    if (response.data.token) {
      // Store the session token
      localStorage.setItem('token', response.data.token)
      // Store user data from customerData
      localStorage.setItem('user', JSON.stringify(response.data.customerData))
    }
    return response.data
  },
  
  logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  },
  
  // Add or update these methods in your authService

  isAuthenticated() {
    const token = localStorage.getItem('token');
    if (!token) return false;
    
    // Optional: Add token expiration check if your tokens include exp
    try {
      // Get expiration if using JWT
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const payload = JSON.parse(window.atob(base64));
      
      if (payload.exp) {
        return payload.exp * 1000 > Date.now();
      }
      
      return true;
    } catch (e) {
      console.error('Error checking token:', e);
      return true; // Assume valid if can't parse
    }
  },

  getCurrentUser() {
    try {
      const userData = localStorage.getItem('user');
      if (!userData) return null;
      
      const user = JSON.parse(userData);
      // Ensure role is always a string
      if (user && user.role !== undefined) {
        user.role = String(user.role);
      }
      return user;
    } catch (e) {
      console.error('Error getting current user:', e);
      return null;
    }
  },

  getUserRole() {
    const user = this.getCurrentUser()
    if (!user) return null
    // Always ensure we return a string
    return String(user.role || '1')
  },
  
  isAdmin() {
    return this.getUserRole() === '2'
  },
  
  isCustomer() {
    return this.getUserRole() === '1'
  },
  
  hasAccess(requiredRole) {
    const userRole = this.getUserRole()
    if (!userRole) return false
    
    // Admin has access to everything
    if (userRole === '2') return true
    
    // Otherwise, check role match
    return userRole === requiredRole
  }
}