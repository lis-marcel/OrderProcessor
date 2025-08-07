<template>
  <div class="all-orders-container">
    <div class="header-actions">
      <h1>All Orders</h1>
    </div>
   
    <!-- Loading indicator -->
    <div v-if="loading" class="loading">
      <p>Loading orders...</p>
    </div>
    
    <!-- Error message -->
    <div v-else-if="error" class="error">
      <p>Error: {{ error }}</p>
      <button @click="fetchAllOrders">Try Again</button>
    </div>

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
    
    <div v-if="orders.length > 0" class="section">
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
              <tr v-for="order in orders" :key="order.id" :class="getOrderStatusClass(order.status)">
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
                  <div class="action-buttons">
                    <button 
                      @click="editOrder(order.id)" 
                      class="btn btn-primary btn-sm"
                      title="Edit Order"
                    >
                      Edit
                    </button>
                    <button class="btn btn-secondary btn-sm" title="Track Order">
                      Track
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
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
        
        if (!token) {
          throw new Error('Authentication token not found');
        }
        
        console.log('Making API request to fetch all orders');
        
        const response = await axios.get('https://127.0.0.1:7092/api/orders/all-orders', {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        });
        
        this.orders = response.data.data;
      } catch (err) {
        console.error('Error fetching all orders:', err);
        this.error = err.message || 'Failed to load orders. Please try again.';
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
    },

    editOrder(orderId) {
      this.$router.push(`/edit-order/${orderId}`);
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
  
.table-container {
  overflow-x: auto;
}

.orders-table {
  min-width: 800px;
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

.badge-processing {
  background-color: #fff3cd;
  color: #856404;
  border: 1px solid #ffeaa7;
}

.badge-shipped {
  background-color: #d1ecf1;
  color: #0c5460;
  border: 1px solid #bee5eb;
}

.badge-delivered {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.badge-canceled {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

/* Action buttons */
.action-buttons {
  display: flex;
  gap: 0.5rem;
  align-items: center;
}

.btn {
  padding: 0.375rem 0.75rem;
  border: none;
  border-radius: 4px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  text-decoration: none;
  display: inline-block;
  transition: all 0.15s ease-in-out;
}

.btn-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.75rem;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background-color: #545b62;
}
</style>