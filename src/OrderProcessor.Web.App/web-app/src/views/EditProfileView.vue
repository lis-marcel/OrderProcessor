<template>
  <div class="edit-profile-container">
    <h1>Edit Profile</h1>
    
    <!-- Success message with return button -->
    <div v-if="successMessage" class="success-message">
      <p>{{ successMessage }}</p>
      <div class="success-actions">
        <router-link to="/user-account" class="btn btn-primary">Return to Account</router-link>
        <button class="btn btn-text" @click="continueEditing">Continue Editing</button>
      </div>
    </div>
    
    <!-- Loading state -->
    <div v-if="loading" class="loading-indicator">
      <p>Loading profile information...</p>
    </div>
    
    <!-- Error state -->
    <div v-else-if="error" class="error-message">
      <p>{{ error }}</p>
      <button @click="loadUserData" class="btn">Try Again</button>
    </div>
    
    <!-- Not logged in state -->
    <div v-else-if="!userData" class="not-logged-in">
      <p>You need to be logged in to edit your profile.</p>
      <div class="auth-buttons">
        <router-link to="/login" class="btn btn-primary">Login</router-link>
      </div>
    </div>
    
    <!-- Edit form - Changed from v-else to v-else-if with a positive condition -->
    <div v-else-if="userData" class="profile-edit-card">
      <!-- Error message -->
      <div v-if="actionError" class="form-error-message">
        <p>{{ actionError }}</p>
      </div>
      
      <form @submit.prevent="saveProfile">
        <div class="form-group">
          <label for="currentName">Current Name</label>
          <input 
            id="currentName" 
            type="text" 
            v-model="profileForm.currentName" 
            class="form-control" 
            disabled
          >
        </div>
        
        <div class="form-group">
          <label for="newName">New Name</label>
          <input 
            id="newName" 
            type="text" 
            v-model="profileForm.newName" 
            class="form-control"
            :class="{ 'is-invalid': validationErrors.newName }"
          >
          <div v-if="validationErrors.newName" class="invalid-feedback">
            {{ validationErrors.newName }}
          </div>
        </div>
        
        <div class="form-group">
          <label for="currentEmail">Current Email</label>
          <input 
            id="currentEmail" 
            type="email" 
            v-model="profileForm.currentEmail" 
            class="form-control" 
            disabled
          >
        </div>
        
        <div class="form-group">
          <label for="newEmail">New Email</label>
          <input 
            id="newEmail" 
            type="email" 
            v-model="profileForm.newEmail" 
            class="form-control"
            :class="{ 'is-invalid': validationErrors.newEmail }"
          >
          <div v-if="validationErrors.newEmail" class="invalid-feedback">
            {{ validationErrors.newEmail }}
          </div>
        </div>
        
        <div class="form-actions">
          <button type="submit" class="btn btn-success" :disabled="saving">
            {{ saving ? 'Saving...' : 'Save Changes' }}
          </button>
          <router-link to="/account" class="btn btn-secondary">Cancel</router-link>
        </div>
      </form>
      
      <!-- Saving indicator overlay -->
      <div v-if="saving" class="saving-overlay">
        <div class="saving-spinner"></div>
        <p>Saving changes...</p>
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/authService.js'

export default {
  name: 'EditProfileView',
  data() {
    return {
      userData: null,
      loading: true,
      saving: false,
      error: null,
      actionError: null,
      successMessage: null,
      profileForm: {
        currentName: '',
        newName: '',
        currentEmail: '',
        newEmail: ''
      },
      validationErrors: {}
    }
  },
  created() {
    this.loadUserData()
  },
  methods: {
    loadUserData() {
      this.loading = true
      this.error = null
      this.actionError = null
      this.successMessage = null
      
      try {
        // Get user data from auth service
        const userData = authService.getCurrentUser()
        
        if (!userData) {
          this.error = null
          this.userData = null
        } else {
          this.userData = userData
          this.profileForm = {
            currentName: userData.name,
            newName: userData.name,
            currentEmail: userData.email,
            newEmail: userData.email
          }
        }
      } catch (err) {
        console.error('Error loading user data:', err)
        this.error = 'Failed to load profile information.'
      } finally {
        this.loading = false
      }
    },
    
    validateForm() {
      const errors = {}
      
      if (!this.profileForm.newName || this.profileForm.newName.trim() === '') {
        errors.newName = 'Name cannot be empty'
      }
      
      if (!this.profileForm.newEmail || this.profileForm.newEmail.trim() === '') {
        errors.newEmail = 'Email cannot be empty'
      } else if (!this.isValidEmail(this.profileForm.newEmail)) {
        errors.newEmail = 'Please enter a valid email address'
      }
      
      this.validationErrors = errors
      return Object.keys(errors).length === 0
    },
    
    isValidEmail(email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
      return emailRegex.test(email)
    },
    
    continueEditing() {
      this.successMessage = null
    },
    
    async saveProfile() {
      this.actionError = null
      this.successMessage = null
      
      if (!this.validateForm()) {
        return
      }
      
      this.saving = true
      
      try {
        // Call API to update user profile
        await authService.updateUserProfile(this.profileForm)
        
        // Update local user data with new values
        this.userData.name = this.profileForm.newName
        this.userData.email = this.profileForm.newEmail
        
        // Save updated user data in auth service
        authService.updateCurrentUser(this.userData)
        
        // Show success message but don't redirect automatically
        this.successMessage = 'Profile updated successfully!'
      } catch (error) {
        console.error('Failed to update profile:', error)
        this.actionError = 'Failed to update profile. Please try again.'
      } finally {
        this.saving = false
      }
    }
  }
}
</script>

<style scoped>
.edit-profile-container {
  max-width: 600px;
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

.profile-edit-card {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  padding: 2rem;
  position: relative;
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #495057;
}

.form-control {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ced4da;
  border-radius: 4px;
  font-size: 1rem;
}

.form-control:disabled {
  background-color: #e9ecef;
  cursor: not-allowed;
}

.form-control:focus {
  outline: none;
  border-color: #80bdff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

.form-control.is-invalid {
  border-color: #dc3545;
}

.invalid-feedback {
  color: #dc3545;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 2rem;
}

.btn {
  padding: 0.5rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.95rem;
  font-weight: 500;
  transition: background-color 0.2s, transform 0.1s;
  text-decoration: none;
  display: inline-block;
  text-align: center;
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

.btn-success {
  background-color: #28a745;
  color: white;
}

.btn-success:hover {
  background-color: #218838;
}

.btn:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}

.saving-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(255, 255, 255, 0.8);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  z-index: 10;
  border-radius: 8px;
}

.saving-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #4CAF50;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

.success-message {
  background-color: #d4edda;
  border-left: 4px solid #28a745;
  color: #155724;
  padding: 1rem;
  margin-bottom: 1.5rem;
  border-radius: 4px;
  text-align: center;
}

.success-actions {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-top: 1rem;
}

.btn-text {
  background: none;
  color: #4CAF50;
  text-decoration: underline;
  padding: 0.5rem;
}

.btn-text:hover {
  color: #2e7d32;
  background-color: transparent;
}

.form-error-message {
  background-color: #f8d7da;
  border-left: 4px solid #dc3545;
  color: #721c24;
  padding: 1rem;
  margin-bottom: 1.5rem;
  border-radius: 4px;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
