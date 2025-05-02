<template>
  <div class="all-orders-container">
    <h1>All Orders</h1>
    
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

export default {
  name: 'AllOrdersView',
  data() {
    return {
      orders: [],
      loading: true,
      error: null
    };
  },
  created() {
    this.fetchOrders();
  },
  methods: {
    async fetchOrders() {
      this.loading = true;
      this.error = null;
      
      try {
        const response = await axios.post('https://127.0.0.1:7092/api/orders', {
          // Your request payload here
          // page: 1,
          // pageSize: 50
        });
        
        this.orders = response.data;
      } catch (error) {
        console.error('Error fetching orders:', error);
        this.error = error.message || 'Failed to load orders';
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
      // Map status codes to human-readable text
      const statuses = {
        0: 'New',
        1: 'Processing',
        2: 'Shipped',
        3: 'Delivered',
        4: 'Canceled'
      };
      return statuses[statusCode] || `Unknown (${statusCode})`;
    },
    getPaymentMethodText(methodCode) {
      // Map payment method codes to human-readable text
      const methods = {
        0: 'Credit Card',
        1: 'Bank Transfer',
        2: 'Cash on Delivery',
        3: 'PayPal'
      };
      return methods[methodCode] || `Unknown (${methodCode})`;
    }
  }
};
</script>

<style scoped>
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