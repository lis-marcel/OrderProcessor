<template>
  <div class="auth-container">
    <h1>Create Account</h1>
    <div class="auth-form">
      <div class="form-group">
        <label for="name">Full Name</label>
        <input 
          type="text" 
          id="name" 
          v-model="registerData.name" 
          required
          :class="{ 'error': validationErrors.name }"
        >
        <div v-if="validationErrors.name" class="error-message">
          {{ validationErrors.name }}
        </div>
      </div>
      
      <div class="form-group">
        <label for="email">Email</label>
        <input 
          type="email" 
          id="email" 
          v-model="registerData.email" 
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
          v-model="registerData.password" 
          required
          :class="{ 'error': validationErrors.password }"
        >
        <div v-if="validationErrors.password" class="error-message">
          {{ validationErrors.password }}
        </div>
      </div>
      
      <div class="form-group">
        <label for="confirmPassword">Confirm Password</label>
        <input 
          type="password" 
          id="confirmPassword" 
          v-model="registerData.confirmPassword" 
          required
          :class="{ 'error': validationErrors.confirmPassword }"
        >
        <div v-if="validationErrors.confirmPassword" class="error-message">
          {{ validationErrors.confirmPassword }}
        </div>
      </div>
      
      <div v-if="registerError" class="error-message register-error">
        {{ registerError }}
      </div>
      
      <div class="form-actions">
        <button 
          @click="register" 
          class="btn btn-primary"
          :disabled="isLoading"
        >
          {{ isLoading ? 'Creating account...' : 'Register' }}
        </button>
      </div>
      
      <div class="auth-links">
        <p>Already have an account? <router-link to="/login">Login</router-link></p>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'RegisterView',
  data() {
    return {
      registerData: {
        name: '',
        email: '',
        password: '',
        confirmPassword: ''
      },
      validationErrors: {},
      registerError: null,
      isLoading: false
    };
  },
  methods: {
    async register() {
      if (!this.validate()) return;
      
      this.isLoading = true;
      this.registerError = null;
      
      try {
        const response = await axios.post('https://127.0.0.1:7092/api/auth/register', {
          name: this.registerData.name,
          email: this.registerData.email,
          password: this.registerData.password
        });
        
        // Store the JWT token
        console.log('Registration successful:', response.data);
        
        // Redirect to login page
        this.$router.push('/login');
      } catch (error) {
        console.error('Registration failed:', error);
        this.registerError = error.response?.data?.message || 'Registration failed. Please try again.';
      } finally {
        this.isLoading = false;
      }
    },
    
    validate() {
      this.validationErrors = {};
      let isValid = true;
      
      if (!this.registerData.name) {
        this.validationErrors.name = 'Name is required';
        isValid = false;
      }
      
      if (!this.registerData.email) {
        this.validationErrors.email = 'Email is required';
        isValid = false;
      } else if (!/\S+@\S+\.\S+/.test(this.registerData.email)) {
        this.validationErrors.email = 'Please enter a valid email address';
        isValid = false;
      }
      
      if (!this.registerData.password) {
        this.validationErrors.password = 'Password is required';
        isValid = false;
      } else if (this.registerData.password.length < 6) {
        this.validationErrors.password = 'Password must be at least 6 characters';
        isValid = false;
      }
      
      if (!this.registerData.confirmPassword) {
        this.validationErrors.confirmPassword = 'Please confirm your password';
        isValid = false;
      } else if (this.registerData.confirmPassword !== this.registerData.password) {
        this.validationErrors.confirmPassword = 'Passwords do not match';
        isValid = false;
      }
      
      return isValid;
    }
  }
};
</script>

<style scoped>
/* Same styles as LoginView */
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

.register-error {
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