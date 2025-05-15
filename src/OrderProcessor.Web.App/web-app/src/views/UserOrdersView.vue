<template>
  <div class="user-orders-container">
    <h1>Your Orders</h1>
    
    <!-- Loading state -->
    <div v-if="loading" class="loading-indicator">
      <p>Loading your orders...</p>
    </div>
    
    <!-- Error state -->
    <div v-else-if="error" class="error-message">
      <p>{{ error }}</p>
      <button @click="fetchUserOrders" class="btn">Try Again</button>
    </div>
    
    <!-- User not logged in state -->
    <div v-else-if="!isAuthenticated" class="not-logged-in">
      <p>You need to be logged in to view your orders.</p>
      <div class="auth-buttons">
        <router-link to="/login" class="btn btn-primary">Login</router-link>
        <router-link to="/register" class="btn btn-secondary">Register</router-link>
      </div>
    </div>
    
    <!-- No orders state -->
    <div v-else-if="orders.length === 0" class="no-orders">
      <p>You don't have any orders yet.</p>
      <router-link to="/create-order" class="btn btn-primary">Create Your First Order</router-link>
    </div>
    
    <!-- Orders display -->
    <div v-else>
      <div class="order-stats">
        <div class="stat-card">
          <div class="stat-value">{{ orders.length }}</div>
          <div class="stat-label">Total Orders</div>
        </div>
        <div class="stat-card">
          <div class="stat-value">{{ formatCurrency(getTotalOrderValue()) }}</div>
          <div class="stat-label">Total Value</div>
        </div>
        <div class="stat-card">
          <div class="stat-value">{{ getActiveOrdersCount() }}</div>
          <div class="stat-label">Active Orders</div>
        </div>
      </div>
      
      <div class="orders-grid">
        <div v-for="order in orders" :key="order.id" class="order-card">
          <div class="order-header" :class="getOrderStatusClass(order.status)">
            <h3 class="order-title">{{ order.productName }}</h3>
            <div class="order-status">{{ getStatusText(order.status) }}</div>
          </div>
          
          <div class="order-body">
            <p><strong>Order ID:</strong> #{{ order.id }}</p>
            <p><strong>Value:</strong> {{ formatCurrency(order.value) }}</p>
            <p><strong>Quantity:</strong> {{ order.quantity }}</p>
            <p><strong>Total:</strong> {{ formatCurrency(order.value * order.quantity) }}</p>
            <p><strong>Shipping Address:</strong></p>
            <p class="address">{{ order.shippingAddress }}</p>
            <p><strong>Created:</strong> {{ formatDate(order.creationTime) }}</p>
            <p v-if="order.markToShippingAt">
              <strong>Shipping Date:</strong> {{ formatDate(order.markToShippingAt) }}
            </p>
            <p><strong>Payment Method:</strong> {{ getPaymentMethodText(order.paymentMethod) }}</p>
          </div>
          
          <div class="order-actions">
            <button class="btn btn-secondary btn-sm">Track Order</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import authService from '../services/authService.js';

export default {
  name: 'UserOrdersView',
  data() {
    return {
      orders: [],
      loading: true,
      error: null,
      isAuthenticated: false
    };
  },
  created() {
    this.isAuthenticated = authService.isAuthenticated();
    if (this.isAuthenticated) {
      this.fetchUserOrders();
    } else {
      this.loading = false;
    }
  },
  methods: {
    async fetchUserOrders() {
      this.loading = true;
      this.error = null;
      
      try {
        const userData = authService.getCurrentUser();
        if (!userData) {
          this.isAuthenticated = false;
          this.loading = false;
          return;
        }
        
        const customerEmail = userData.email;
        
        // Get authentication token
        const token = localStorage.getItem('token');
        
        // Make API request with customer ID
        const response = await axios.post('https://127.0.0.1:7092/api/customer/customer-orders', 
          { customerEmail },
          {
            headers: {
              'Authorization': `Bearer ${token}`
            }
          }
        );
        
        this.orders = response.data;
      } catch (err) {
        console.error('Error fetching user orders:', err);
        this.error = err.message || 'Failed to load your orders. Please try again.';
      } finally {
        this.loading = false;
      }
    },
    
    formatCurrency(value) {
      return new Intl.NumberFormat('pl-PL', { 
        style: 'currency', 
        currency: 'PLN' 
      }).format(value);
    },
    
    formatDate(dateString) {
      if (!dateString) return 'N/A';
      const date = new Date(dateString);
      return new Intl.DateTimeFormat('pl-PL', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }).format(date);
    },
    
    getStatusText(statusCode) {
      const statuses = {
        1: 'New',
        2: 'Processing',
        3: 'Shipped',
        4: 'Delivered',
        5: 'Canceled'
      };
      return statuses[statusCode] || `Unknown (${statusCode})`;
    },
    
    getPaymentMethodText(methodCode) {
      const methods = {
        1: 'Cash on Delivery',
        2: 'Credit Card',
      };
      return methods[methodCode] || `Unknown (${methodCode})`;
    },
    
    getOrderStatusClass(statusCode) {
      const classes = {
        0: 'status-new',
        1: 'status-processing',
        2: 'status-shipped',
        3: 'status-delivered',
        4: 'status-canceled'
      };
      return classes[statusCode] || '';
    },
    
    getTotalOrderValue() {
      return this.orders.reduce((total, order) => {
        return total + (order.value * order.quantity);
      }, 0);
    },
    
    getActiveOrdersCount() {
      return this.orders.filter(order => order.status !== 3 && order.status !== 4).length;
    }
  }
};
</script>

<style scoped>
.user-orders-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

h1 {
  text-align: center;
  margin-bottom: 2rem;
}

.loading-indicator,
.error-message,
.not-logged-in,
.no-orders {
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

.order-stats {
  display: flex;
  justify-content: space-between;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.stat-card {
  flex: 1;
  min-width: 200px;
  background-color: #fff;
  padding: 1.25rem;
  border-radius: 8px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.stat-value {
  font-size: 1.75rem;
  font-weight: bold;
  color: #4CAF50;
  margin-bottom: 0.5rem;
}

.stat-label {
  color: #6c757d;
  font-size: 0.95rem;
}

.orders-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 1.5rem;
}

.order-card {
  background-color: #fff;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s ease-in-out;
}

.order-card:hover {
  transform: translateY(-5px);
}

.order-header {
  padding: 1rem 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #eee;
}

.order-title {
  margin: 0;
  font-size: 1.25rem;
  font-weight: 600;
}

.order-status {
  padding: 0.25rem 0.75rem;
  border-radius: 50px;
  font-size: 0.85rem;
  font-weight: 500;
  color: white;
}

.status-new {
  background-color: #17a2b8;
}

.status-new .order-status {
  background-color: #17a2b8;
}

.status-processing {
  background-color: #ffc107;
}

.status-processing .order-status {
  background-color: #ffc107;
  color: #212529;
}

.status-shipped {
  background-color: #007bff;
}

.status-shipped .order-status {
  background-color: #007bff;
}

.status-delivered {
  background-color: #28a745;
}

.status-delivered .order-status {
  background-color: #28a745;
}

.status-canceled {
  background-color: #dc3545;
}

.status-canceled .order-status {
  background-color: #dc3545;
}

.order-body {
  padding: 1.5rem;
}

.address {
  margin-top: 0;
  padding-left: 1rem;
  color: #6c757d;
  font-style: italic;
}

.order-actions {
  padding: 1rem 1.5rem;
  border-top: 1px solid #eee;
  display: flex;
  justify-content: flex-end;
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

.btn-sm {
  padding: 0.35rem 0.65rem;
  font-size: 0.85rem;
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

@media (max-width: 768px) {
  .orders-grid {
    grid-template-columns: 1fr;
  }
}
</style>