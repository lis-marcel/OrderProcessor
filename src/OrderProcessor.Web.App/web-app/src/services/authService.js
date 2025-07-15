// src/services/authService.js
import axios from 'axios'

const API_AUTH_URL = 'https://127.0.0.1:7092/api/auth/'

export default {
  // In the login method, modify how user data is stored
  async login(email, password) {
    const response = await axios.post(API_AUTH_URL + 'login', { email, password })
    if (response.data.token) {
      // Store the session token
      localStorage.setItem('token', response.data.token)
      
      // Prepare user data - use the user object from response
      const userData = response.data.user
      console.log('Server response:', response.data)
      
      // Store user data with proper role mapping
      localStorage.setItem('user', JSON.stringify(userData))
    }
    return response.data
  },

  // Update getCurrentUser to handle the correct role field
  getCurrentUser() {
    try {
      const userData = localStorage.getItem('user');
      if (!userData) return null;
      
      // Check if the data looks like valid JSON
      if (!userData.startsWith('{') && !userData.startsWith('[')) {
        console.error('Invalid JSON data in localStorage, clearing...');
        localStorage.removeItem('user');
        return null;
      }
      
      const user = JSON.parse(userData);
      
      // Debug what we got from storage
      console.log('Raw user data from storage:', user);
      
      return user;
    } catch (e) {
      console.error('Error getting current user', e);
 
      return null;
    }
  },

  async logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  },

  // Update the role check methods to use accountType instead
  getUserRole() {
    const user = this.getCurrentUser()
    if (!user) return null
    
    // Use accountType as the role indicator
    return String(user.accountType || '1')
  },

  isAdmin() {
    const role = this.getUserRole()
    console.log('Role check in isAdmin():', role)
    return role === '2'
  },

  isCustomer() {
    const role = this.getUserRole()
    return role === '1'
  },

  isAuthenticated() {
    return !!localStorage.getItem('token')
  },
}