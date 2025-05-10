<template>
  <div class="auth-container">
    <h1>Login</h1>
    <div class="auth-form">
      <div class="form-group">
        <label for="email">Email</label>
        <input 
          type="email" 
          id="email" 
          v-model="loginData.email" 
          required
          :class="{ 'error': validationErrors.email }"
        >
        <div v-if="validationErrors.email" class="error-message">
          {{ validationErrors.email }}
        </div>
      </div>
      
      <div class="form-group">
        <label for="password">Password</label>
        <input 
          type="password" 
          id="password" 
          v-model="loginData.password" 
          required
          :class="{ 'error': validationErrors.password }"
        >
        <div v-if="validationErrors.password" class="error-message">
          {{ validationErrors.password }}
        </div>
      </div>
      
      <div v-if="loginError" class="error-message login-error">
        {{ loginError }}
      </div>
      
      <div class="form-actions">
        <button 
          @click="login" 
          class="btn btn-primary"
          :disabled="isLoading"
        >
          {{ isLoading ? 'Logging in...' : 'Login' }}
        </button>
      </div>
      
      <div class="auth-links">
        <p>Don't have an account? <router-link to="/register">Register</router-link></p>
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/authService.js';

export default {
  name: 'LoginView',
  data() {
    return {
      loginData: {
        email: '',
        password: ''
      },
      validationErrors: {},
      loginError: null,
      isLoading: false
    };
  },
  methods: {
    async login() {
      if (!this.validate()) return;
      
      this.isLoading = true;
      this.loginError = null;
      
      try {
        await authService.login(this.loginData.email, this.loginData.password);
        this.$router.push('/');
      } catch (error) {
        console.error('Login failed:', error);
        this.loginError = error.response?.data?.message || 'Login failed. Please check your credentials.';
      } finally {
        this.isLoading = false;
      }
    },
    
    validate() {
      this.validationErrors = {};
      let isValid = true;
      
      if (!this.loginData.email) {
        this.validationErrors.email = 'Email is required';
        isValid = false;
      } else if (!/\S+@\S+\.\S+/.test(this.loginData.email)) {
        this.validationErrors.email = 'Please enter a valid email address';
        isValid = false;
      }
      
      if (!this.loginData.password) {
        this.validationErrors.password = 'Password is required';
        isValid = false;
      }
      
      return isValid;
    }
  }
};
</script>

<style scoped>
.auth-container {
  max-width: 500px;
  margin: 0 auto;
  padding: 2rem;
}

h1 {
  text-align: center;
  margin-bottom: 2rem;
}

.auth-form {
  background-color: #f9f9f9;
  border-radius: 8px;
  padding: 2rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
}

.form-group input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

.form-group input.error {
  border-color: #ff0000;
}

.error-message {
  color: #ff0000;
  font-size: 0.85rem;
  margin-top: 0.25rem;
}

.login-error {
  margin-bottom: 1rem;
  padding: 0.5rem;
  background-color: rgba(255, 0, 0, 0.1);
  border-radius: 4px;
  text-align: center;
}

.form-actions {
  margin-top: 1rem;
}

.btn {
  width: 100%;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  font-weight: bold;
}

.btn-primary {
  background-color: #3498db;
  color: white;
}

.btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.auth-links {
  margin-top: 1.5rem;
  text-align: center;
}

.auth-links a {
  color: #3498db;
  text-decoration: none;
}

.auth-links a:hover {
  text-decoration: underline;
}
</style>