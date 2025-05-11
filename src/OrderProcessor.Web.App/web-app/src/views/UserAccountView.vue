<template>
  <div class="user-account-container">
    <h1>Your Account</h1>
    
    <!-- Loading state -->
    <div v-if="loading" class="loading-indicator">
      <p>Loading account information...</p>
    </div>
    
    <!-- Error state -->
    <div v-else-if="error" class="error-message">
      <p>{{ error }}</p>
      <button @click="loadUserData" class="btn">Try Again</button>
    </div>
    
    <!-- User not logged in state -->
    <div v-else-if="!userData" class="not-logged-in">
      <p>You need to be logged in to view your account information.</p>
      <div class="auth-buttons">
        <router-link to="/login" class="btn btn-primary">Login</router-link>
        <router-link to="/register" class="btn btn-secondary">Register</router-link>
      </div>
    </div>
    
    <!-- User data display -->
    <div v-else class="user-profile-card">
      <div class="profile-header">
        <div class="avatar">
          {{ getUserInitials() }}
        </div>
        <div class="user-name">
          <h2>{{ userData.name }}</h2>
          <span class="customer-type">{{ getCustomerTypeText(userData.customerType) }}</span>
        </div>
      </div>
      
      <div class="profile-details">
        <div class="detail-item">
          <div class="detail-label">Email</div>
          <div class="detail-value">{{ userData.email }}</div>
        </div>
        
        <div class="detail-item">
          <div class="detail-label">Account ID</div>
          <div class="detail-value">{{ userData.id }}</div>
        </div>
        
        <div class="detail-item">
          <div class="detail-label">Last Login</div>
          <div class="detail-value">{{ formatDate(userData.lastLoginAt) }}</div>
        </div>
      </div>
      
      <div class="profile-actions">
        <button class="btn btn-primary" @click="editProfile">Edit Profile</button>
        <button class="btn btn-secondary" @click="changePassword">Change Password</button>
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/authService.js'

export default {
  name: 'UserAccountView',
  data() {
    return {
      userData: null,
      loading: true,
      error: null,
      editMode: false
    }
  },
  created() {
    this.loadUserData()
  },
  methods: {
    loadUserData() {
      this.loading = true
      this.error = null
      
      try {
        // Get user data from auth service
        const userData = authService.getCurrentUser()
        
        if (!userData) {
          this.error = null
          this.userData = null
        } else {
          this.userData = userData
        }
      } catch (err) {
        console.error('Error loading user data:', err)
        this.error = 'Failed to load account information.'
      } finally {
        this.loading = false
      }
    },
    
    getUserInitials() {
      if (!this.userData || !this.userData.name) return '?'
      
      return this.userData.name
        .split(' ')
        .map(n => n[0])
        .join('')
        .toUpperCase()
        .substring(0, 2)
    },
    
    getCustomerTypeText(customerType) {
      const types = {
        1: 'Private',
        2: 'Company',
      }
      return types[customerType] || 'Customer'
    },
    
    formatDate(dateString) {
      if (!dateString) return 'Never'
      
      const date = new Date(dateString)
      return new Intl.DateTimeFormat('pl-PL', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }).format(date)
    },
    
    editProfile() {
      // Placeholder for future implementation
      alert('Profile editing will be implemented in a future update.')
    },
    
    changePassword() {
      // Placeholder for future implementation
      alert('Password change will be implemented in a future update.')
    }
  }
}
</script>

<style scoped>
.user-account-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem;
}

h1 {
  text-align: center;
  margin-bottom: 2rem;
}

.loading-indicator,
.error-message,
.not-logged-in {
  text-align: center;
  margin: 2rem 0;
  padding: 1.5rem;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.error-message {
  color: #d9534f;
  border-left: 4px solid #d9534f;
}

.auth-buttons {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-top: 1.5rem;
}

.user-profile-card {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.profile-header {
  display: flex;
  align-items: center;
  padding: 1.5rem;
  background-color: #f8f9fa;
  border-bottom: 1px solid #e9ecef;
}

.avatar {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background-color: #4CAF50;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: bold;
  margin-right: 1rem;
}

.user-name {
  display: flex;
  flex-direction: column;
}

.user-name h2 {
  margin: 0;
  font-size: 1.5rem;
}

.customer-type {
  font-size: 0.9rem;
  color: #6c757d;
  margin-top: 0.25rem;
}

.profile-details {
  padding: 1.5rem;
}

.detail-item {
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid #eee;
  display: flex;
  flex-wrap: wrap;
}

.detail-item:last-child {
  margin-bottom: 0;
  padding-bottom: 0;
  border-bottom: none;
}

.detail-label {
  font-weight: bold;
  color: #495057;
  width: 150px;
}

.detail-value {
  flex: 1;
  color: #212529;
}

.profile-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding: 1rem 1.5rem;
  background-color: #f8f9fa;
  border-top: 1px solid #e9ecef;
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.95rem;
  font-weight: 500;
  transition: background-color 0.2s, transform 0.1s;
}

.btn:active {
  transform: translateY(1px);
}

.btn-primary {
  background-color: #4CAF50;
  color: white;
}

.btn-primary:hover {
  background-color: #45a049;
}

.btn-secondary {
  background-color: #e9ecef;
  color: #495057;
}

.btn-secondary:hover {
  background-color: #dee2e6;
}
</style>