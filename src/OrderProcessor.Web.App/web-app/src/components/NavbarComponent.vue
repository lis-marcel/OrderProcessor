<template>
    <nav>
        <ul class="navbar">
            <li v-if="isAuthenticated && isAdmin"><router-link to="/all-orders">All Orders</router-link></li>
            <li v-if="isAuthenticated && !isAdmin"><router-link to="/user-orders">My Orders</router-link></li>
            <li v-if="isAuthenticated"><router-link to="/user-account">My Account</router-link></li>
            <li class="auth-links">
              <template v-if="isAuthenticated">
                <span class="user-name">{{ userName }}</span>
                <span v-if="isAdmin" class="admin-badge">Admin</span>
                <span v-if="!isAdmin" class="customer-badge">Customer</span>
                <button @click="logout" class="logout-btn">Logout</button>
              </template>
              <template v-else>
                <router-link to="/login" class="login-link">Login</router-link>
                <router-link to="/register" class="register-link">Register</router-link>
              </template>
            </li>
        </ul>
    </nav>
</template>

<script>
import authService from '../services/authService.js'

export default {
  name: 'NavbarComponent',
  data() {
    return {
      isAuthenticated: false,
      userName: 'User'
    }
  },
  methods: {
    logout() {
      authService.logout()
      this.updateAuthState()
      this.$router.push('/login')
    },
    
    updateAuthState() {
      const userData = authService.getCurrentUser();
      this.isAuthenticated = authService.isAuthenticated();
      this.isAdmin = authService.isAdmin();
      this.userName = userData?.name || 'User';
      
      console.log('Navbar auth state updated:', {
        userData: userData ? {
          name: userData.name,
          accountType: userData.accountType
        } : null,
        isAuthenticated: this.isAuthenticated,
        isAdmin: this.isAdmin
      });
    }
  },
  created() {
    // Initialize auth state
    this.updateAuthState()
    
    // Listen for route changes to update auth state
    this.$watch(
      () => this.$route,
      () => this.updateAuthState(),
      { immediate: true }
    )
    
    // Keep auth state updated across tabs
    window.addEventListener('storage', () => {
      this.updateAuthState()
    })
  },
  beforeUnmount() {
    // Clean up event listeners
    window.removeEventListener('storage', this.updateAuthState)
  }
}
</script>

<style>
.navbar {
  list-style-type: none;
  margin: 0;
  padding: 0;
  display: flex;
  background-color: #333;
  align-items: center;
}

.navbar li {
  padding: 14px 20px;
}

.navbar li a {
  color: white;
  text-decoration: none;
}

.navbar li a:hover {
  background-color: #111;
}

.navbar li a.active {
  background-color: #4CAF50;
  color: white;
}

.navbar li a:visited {
  color: white;
}

.navbar li a:focus {
  outline: none;
}

.auth-links {
  margin-left: auto;
  display: flex;
  align-items: center;
  gap: 10px;
}

.user-name {
  color: white;
  margin-right: 10px;
}

.admin-badge {
  background: #ff9500;
  color: white;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 0.8em;
  font-weight: bold;
  text-transform: uppercase;
  margin-right: 10px;
  border: 1px solid #ff7700;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
}

.customer-badge {
  background: #0044aa;
  color: white;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 0.8em;
  font-weight: bold;
  text-transform: uppercase;
  margin-right: 10px;
  border: 1px solid #00aa;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
}

.logout-btn {
  background: #d9534f;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.login-link, .register-link {
  color: white;
  text-decoration: none;
  margin-left: 10px;
}

.register-link {
  background: #5cb85c;
  padding: 8px 12px;
  border-radius: 4px;
}
</style>