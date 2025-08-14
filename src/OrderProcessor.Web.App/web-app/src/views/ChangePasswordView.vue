<template>
  <div class="change-password-container">
    <h1>Change Password</h1>
    
    <div class="password-card">
      <div v-if="loading" class="loading-indicator">
        <p>Loading account information...</p>
      </div>
      
      <div v-else-if="error" class="error-message">
        <p>{{ error }}</p>
        <button @click="loadUserData" class="btn">Try Again</button>
      </div>
      
      <div v-else-if="!userData" class="not-logged-in">
        <p>You need to be logged in to change your password.</p>
        <div class="auth-buttons">
          <router-link to="/login" class="btn btn-primary">Login</router-link>
        </div>
      </div>
      
      <div v-else>
        <div v-if="passwordChangeError" class="error-message">
          {{ passwordChangeError }}
        </div>
        <div v-if="passwordChangeSuccess" class="success-message">
          {{ passwordChangeSuccess }}
        </div>
        
        <form @submit.prevent="submitPasswordChange" class="password-form">
          <div class="form-group">
            <label for="currentPassword">Current Password</label>
            <input 
              type="password" 
              id="currentPassword"
              v-model="passwordForm.currentPassword"
              required
              class="form-control"
            />
          </div>
          
          <div class="form-group">
            <label for="newPassword">New Password</label>
            <input 
              type="password" 
              id="newPassword"
              v-model="passwordForm.newPassword"
              required
              class="form-control"
            />
          </div>
          
          <div class="form-group">
            <label for="confirmPassword">Confirm New Password</label>
            <input 
              type="password" 
              id="confirmPassword"
              v-model="passwordForm.confirmPassword"
              required
              class="form-control"
            />
            <div v-if="!passwordsMatch" class="validation-error">
              Passwords do not match
            </div>
          </div>
          
          <div class="form-actions">
            <router-link to="/account" class="btn btn-secondary">
              Cancel
            </router-link>
            <button 
              type="submit" 
              class="btn btn-primary" 
              :disabled="isSubmitting || !passwordsMatch"
            >
              {{ isSubmitting ? 'Changing...' : 'Change Password' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/authService.js'

export default {
  name: 'ChangePasswordView',
  data() {
    return {
      userData: null,
      loading: true,
      error: null,
      isSubmitting: false,
      passwordChangeError: null,
      passwordChangeSuccess: null,
      passwordForm: {
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
      }
    }
  },
  created() {
    this.loadUserData()
  },
  computed: {
    passwordsMatch() {
      return this.passwordForm.newPassword === this.passwordForm.confirmPassword;
    }
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
    
    resetForm() {
      this.passwordForm = {
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
      };
      this.passwordChangeError = null;
    },
    
    async submitPasswordChange() {
      if (!this.passwordsMatch) {
        return;
      }
      
      this.isSubmitting = true;
      this.passwordChangeError = null;
      this.passwordChangeSuccess = null;
      
      try {
        const passwordData = {
          email: this.userData.email,
          currentPassword: this.passwordForm.currentPassword,
          newPassword: this.passwordForm.newPassword
        };
        
        await authService.changePassword(passwordData);
        this.passwordChangeSuccess = "Password changed successfully!";
        this.resetForm();
        
        // Redirect back to account page after 2 seconds
        setTimeout(() => {
          this.$router.push('/user-account');
        }, 2000);
      } catch (error) {
        console.error('Password change error:', error);
        this.passwordChangeError = error.message || 'Failed to change password. Please try again.';
      } finally {
        this.isSubmitting = false;
      }
    }
  }
}
</script>

<style scoped>
.change-password-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 2rem;
}

h1 {
  text-align: center;
  margin-bottom: 2rem;
}

.password-card {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  padding: 2rem;
}

.loading-indicator,
.error-message,
.not-logged-in {
  text-align: center;
  margin: 1rem 0;
  padding: 1rem;
  background-color: #f9f9f9;
  border-radius: 8px;
}

.error-message {
  background-color: #f8d7da;
  color: #721c24;
  padding: 0.75rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.success-message {
  background-color: #d4edda;
  color: #155724;
  padding: 0.75rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #495057;
}

.form-control {
  width: 100%;
  padding: 0.75rem;
  font-size: 1rem;
  border: 1px solid #ced4da;
  border-radius: 4px;
}

.validation-error {
  color: #dc3545;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

.form-actions {
  display: flex;
  justify-content: space-between;
  margin-top: 2rem;
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.95rem;
  font-weight: 500;
  transition: background-color 0.2s, transform 0.1s;
  text-decoration: none;
  display: inline-block;
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
