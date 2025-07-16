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
      
      <!-- New Orders as Tiles -->
      <div v-if="newOrders.length > 0" class="section">
        <h2 class="section-title">New Orders</h2>
        <div class="orders-grid">
          <div v-for="order in newOrders" :key="order.id" class="order-card">
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
      
      <!-- Other Orders as Table -->
      <div v-if="otherOrders.length > 0" class="section">
        <h2 class="section-title">Order History</h2>
        <div class="table-container">
          <table class="orders-table">
            <thead>
              <tr>
                <th>Order ID</th>
                <th>Product</th>
                <th>Status</th>
                <th>Quantity</th>
                <th>Unit Price</th>
                <th>Total</th>
                <th>Payment</th>
                <th>Created</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="order in otherOrders" :key="order.id" :class="getOrderStatusClass(order.status)">
                <td class="order-id">#{{ order.id }}</td>
                <td class="product-name">{{ order.productName }}</td>
                <td>
                  <span class="status-badge" :class="getStatusBadgeClass(order.status)">
                    {{ getStatusText(order.status) }}
                  </span>
                </td>
                <td>{{ order.quantity }}</td>
                <td>{{ formatCurrency(order.value) }}</td>
                <td class="total-value">{{ formatCurrency(order.value * order.quantity) }}</td>
                <td>{{ getPaymentMethodText(order.paymentMethod) }}</td>
                <td>{{ formatDate(order.creationTime) }}</td>
                <td>
                  <button class="btn btn-secondary btn-sm">Track</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from '../services/axiosConfig.js';
import authService from '../services/authService.js';

const API_ORDERS_URL = 'https://localhost:7092/api/customer/'

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
  computed: {
    newOrders() {
      return this.orders.filter(order => order.status === 1); // Status 1 = New
    },
    otherOrders() {
      return this.orders.filter(order => order.status !== 1); // All other statuses
    }
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
      
        const token = localStorage.getItem('token');
             
        const response = await axios.post(`${API_ORDERS_URL}customer-orders`, 
          {},
          {
            headers: {
              'Authorization': `Bearer ${token}`,
              'Content-Type': 'application/json'
            }
          }
        );
        
        console.log('Orders response:', response.data);
        this.orders = response.data;
      } catch (err) {
        console.error('Error fetching user orders:', err);
        console.error('Error response:', err.response);
        
        if (err.response) {
          // Server responded with error status
          const status = err.response.status;
          const data = err.response.data;
          
          if (status === 403) {
            this.error = 'Access denied. Please log in again.';
          } else if (status === 401) {
            this.error = 'Your session has expired. Please log in again.';
          } else {
            this.error = data?.message || `Server error (${status}). Please try again.`;
          }
        } else if (err.request) {
          // Network error
          this.error = 'Network error. Please check your connection and try again.';
        } else {
          this.error = err.message || 'Failed to load your orders. Please try again.';
        }
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
        2: 'In Warehouse',
        3: 'Pending to Shipping',
        4: 'In Shipping',
        5: 'Returned to Customer',
        6: 'Error',
        7: 'Closed'
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
        1: 'badge-new',
        2: 'badge-in-warehouse',
        3: 'badge-pending-to-shipping',
        4: 'badge-in-shipping',
        5: 'badge-returned-to-customer',
        6: 'badge-error',
        7: 'badge-closed'
      };
      return classes[statusCode] || '';
    },
    
    getStatusBadgeClass(statusCode) {
      const classes = {
        1: 'badge-new',
        2: 'badge-in-warehouse',
        3: 'badge-pending-to-shipping',
        4: 'badge-in-shipping',
        5: 'badge-returned-to-customer',
        6: 'badge-error',
        7: 'badge-closed'
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
  
  .table-container {
    overflow-x: auto;
  }
  
  .orders-table {
    min-width: 800px;
  }
}

/* Section styling */
.section {
  margin-bottom: 3rem;
}

.section-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #4CAF50;
}

/* Table styling */
.table-container {
  background-color: #fff;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.orders-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.9rem;
}

.orders-table th {
  background-color: #f8f9fa;
  color: #495057;
  font-weight: 600;
  padding: 1rem 0.75rem;
  text-align: left;
  border-bottom: 2px solid #dee2e6;
  font-size: 0.85rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.orders-table td {
  padding: 1rem 0.75rem;
  border-bottom: 1px solid #dee2e6;
  vertical-align: middle;
}

.orders-table tbody tr:hover {
  background-color: #f8f9fa;
}

.orders-table tbody tr:last-child td {
  border-bottom: none;
}

.order-id {
  font-family: 'Courier New', monospace;
  font-weight: 600;
  color: #007bff;
}

.product-name {
  font-weight: 500;
  max-width: 200px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.total-value {
  font-weight: 600;
  color: #28a745;
}

/* Status badges for table */
.status-badge {
  padding: 0.25rem 0.5rem;
  border-radius: 12px;
  font-size: 0.75rem;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  display: inline-block;
  min-width: 80px;
  text-align: center;
}

.badge-new {
  background-color: #e3f2fd;
  color: #1976d2;
  border: 1px solid #bbdefb;
}

.badge-in-warehouse {
  background-color: #fff3cd;
  color: #856404;
  border: 1px solid #ffeaa7;
}

.badge-pending-to-shipping {
  background-color: #d1ecf1;
  color: #0c5460;
  border: 1px solid #bee5eb;
}

.badge-in-shipping {
  background-color: #e1f5fe;
  color: #01579b;
  border: 1px solid #b3e5fc;
}

.badge-returned-to-customer {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.badge-error {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.badge-closed {
  background-color: #e2e3e5;
  color: #495057;
  border: 1px solid #ced4da;
}
</style>