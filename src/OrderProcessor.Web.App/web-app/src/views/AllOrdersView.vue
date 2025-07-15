<template>
  <div class="all-orders-container">
    <div class="header-actions">
      <h1>All Orders</h1>
      <router-link to="/create-order" class="create-order-btn">Create New Order</router-link>
    </div>
   
    <!-- Loading indicator -->
    <div v-if="loading" class="loading">
      <p>Loading orders...</p>
    </div>
    
    <!-- Error message -->
    <div v-else-if="error" class="error">
      <p>Error: {{ error }}</p>
      <button @click="fetchOrders">Try Again</button>
    </div>
    
    <!-- Orders grid -->
    <div v-else class="orders-grid">
      <div v-for="order in orders" :key="order.id" class="order-card">
        <h3>{{ order.productName }}</h3>
        <p><strong>Value:</strong> {{ formatCurrency(order.value) }}</p>
        <p><strong>Quantity:</strong> {{ order.quantity }}</p>
        <p><strong>Status:</strong> {{ getStatusText(order.status) }}</p>
        <p><strong>Shipping Address:</strong></p>
        <p class="address">{{ order.shippingAddress }}</p>
        <p><strong>Created:</strong> {{ formatDate(order.creationTime) }}</p>
        <p v-if="order.markToShippingAt">
          <strong>Shipping Date:</strong> {{ formatDate(order.markToShippingAt) }}
        </p>
        <p><strong>Payment:</strong> {{ getPaymentMethodText(order.paymentMethod) }}</p>
        <p><strong>Customer ID:</strong> {{ order.customerId }}</p>
      </div>
    </div>
    
    <!-- No orders message -->
    <div v-if="!loading && !error && orders.length === 0" class="no-orders">
      <p>No orders found</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import authService from '../services/authService.js';

export default {
  name: 'AllOrdersView',
  data() {
    return {
      orders: [],
      loading: true,
      error: null,
      userData: null
    };
  },
  created() {
    // Debug user role
    const userData = authService.getCurrentUser();
    console.log('AllOrdersView created:', {
      userData: userData ? {
        accountType: userData.accountType
      } : null,
      isAdmin: authService.isAdmin()
    });
    
    // Check if admin
    if (!authService.isAdmin()) {
      console.log('Non-admin accessing AllOrdersView, redirecting');
      this.$router.push('/user-orders');
      return;
    }
    
    // Admin can proceed to fetch all orders
    this.fetchAllOrders();
  },
  methods: {
    // Fix: Wrap fetchAllOrders within methods object
    async fetchAllOrders() {
      this.loading = true;
      this.error = null;
      
      try {
        const token = localStorage.getItem('token');
        console.log('Fetching orders with token:', token ? 'Token exists' : 'No token');
        
        if (!token) {
          throw new Error('Authentication token not found');
        }
        
        console.log('Making API request to fetch all orders');
        
        const response = await axios.get('https://127.0.0.1:7092/api/orders/all-orders', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        
        console.log('Orders API response:', response.data);
        this.orders = response.data;
      } catch (err) {
        console.error('Error fetching all orders:', err);
        this.error = err.message || 'Failed to load orders. Please try again.';
      } finally {
        this.loading = false;
      }
    },
    
    // Add missing utility methods used in the template
    formatCurrency(value) {
      if (value == null) return '-';
      return new Intl.NumberFormat('en-US', { 
        style: 'currency', 
        currency: 'USD'
      }).format(value);
    },
    
    formatDate(dateString) {
      if (!dateString) return '-';
      const date = new Date(dateString);
      return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }).format(date);
    },
    
    getStatusText(status) {
      const statusMap = {
        0: 'New',
        1: 'Processing',
        2: 'Shipped',
        3: 'Delivered',
        4: 'Canceled'
      };
      return statusMap[status] || 'Unknown';
    },
    
    getPaymentMethodText(method) {
      const methodMap = {
        0: 'Credit Card',
        1: 'PayPal',
        2: 'Bank Transfer',
        3: 'Cash on Delivery'
      };
      return methodMap[method] || 'Unknown';
    }
  }
};
</script>

<style scoped>
.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.create-order-btn {
  background-color: #4CAF50;
  color: white;
  text-decoration: none;
  padding: 0.75rem 1.25rem;
  border-radius: 4px;
  font-weight: bold;
  transition: background-color 0.2s;
}

.create-order-btn:hover {
  background-color: #3d9c40;
}

.all-orders-container {
  padding: 1rem;
}

.loading, .error, .no-orders {
  text-align: center;
  margin: 2rem 0;
}

.error {
  color: red;
}

.orders-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1rem;
}

@media (max-width: 1200px) {
  .orders-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 900px) {
  .orders-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 600px) {
  .orders-grid {
    grid-template-columns: 1fr;
  }
}

.order-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  background-color: white;
  transition: transform 0.2s, box-shadow 0.2s;
}

.order-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
}

.order-card h3 {
  margin-top: 0;
  color: #333;
  border-bottom: 1px solid #eee;
  padding-bottom: 0.5rem;
}

.address {
  white-space: pre-line;
  color: #555;
  font-style: italic;
}
</style>