<template>
  <div class="edit-order-container">
    <div class="header-actions">
      <h1>Edit Order #{{ orderId }}</h1>
      <div class="header-buttons">
        <button @click="goBack" class="btn btn-secondary">
          ← Back to Orders
        </button>
      </div>
    </div>

    <!-- Loading indicator -->
    <div v-if="loading" class="loading">
      <p>Loading order details...</p>
    </div>

    <!-- Error message -->
    <div v-else-if="error" class="error">
      <p>Error: {{ error }}</p>
      <button @click="fetchOrderDetails">Try Again</button>
    </div>

    <!-- Edit Form -->
    <div v-else-if="order" class="edit-form-container">
      <div class="form-grid">
        <!-- Order Information Section -->
        <div class="form-section">
          <h3 class="section-title">Order Information</h3>
          
          <div class="form-group">
            <label for="orderId">Order ID</label>
            <input 
              id="orderId" 
              type="text" 
              :value="order.id" 
              disabled 
              class="form-control disabled"
            >
          </div>

          <div class="form-group">
            <label for="productName">Product Name</label>
            <input 
              id="productName" 
              v-model="editedOrder.productName" 
              type="text" 
              class="form-control"
              :class="{ 'error': validationErrors.productName }"
            >
            <span v-if="validationErrors.productName" class="error-text">
              {{ validationErrors.productName }}
            </span>
          </div>

          <div class="form-group">
            <label for="shippingAddress">Shipping Address</label>
            <textarea 
              id="shippingAddress" 
              v-model="editedOrder.shippingAddress" 
              class="form-control"
              :class="{ 'error': validationErrors.shippingAddress }"
              rows="3"
              placeholder="Enter shipping address..."
            ></textarea>
            <span v-if="validationErrors.shippingAddress" class="error-text">
              {{ validationErrors.shippingAddress }}
            </span>
          </div>

          <div class="form-group">
            <label for="quantity">Quantity</label>
            <input 
              id="quantity" 
              v-model.number="editedOrder.quantity" 
              type="number" 
              min="1" 
              class="form-control"
              :class="{ 'error': validationErrors.quantity }"
            >
            <span v-if="validationErrors.quantity" class="error-text">
              {{ validationErrors.quantity }}
            </span>
          </div>

          <div class="form-group">
            <label for="value">Unit Price (PLN)</label>
            <input 
              id="value" 
              v-model.number="editedOrder.value" 
              type="number" 
              step="0.01" 
              min="0" 
              class="form-control"
              :class="{ 'error': validationErrors.value }"
            >
            <span v-if="validationErrors.value" class="error-text">
              {{ validationErrors.value }}
            </span>
          </div>

          <div class="form-group">
            <label>Total Value</label>
            <div class="total-display">
              {{ formatCurrency(editedOrder.value * editedOrder.quantity) }}
            </div>
          </div>
        </div>

        <!-- Status and Payment Section -->
        <div class="form-section">
          <h3 class="section-title">Status & Payment</h3>
          
          <div class="form-group">
            <label for="status">Order Status</label>
            <select 
              id="status" 
              v-model="editedOrder.status" 
              class="form-control"
            >
              <option value="1">New</option>
              <option value="2">In Warehouse</option>
              <option value="3">Pending to Shipping</option>
              <option value="4">In Shipping</option>
              <option value="5">Returned to Customer</option>
              <option value="6">Error</option>
              <option value="7">Closed</option>
            </select>
          </div>

          <div class="form-group">
            <label for="paymentMethod">Payment Method</label>
            <select 
              id="paymentMethod" 
              v-model="editedOrder.paymentMethod" 
              class="form-control"
            >
              <option value="1">Cash on Delivery</option>
              <option value="2">Credit Card</option>
            </select>
          </div>

          <div class="form-group">
            <label>Created Date</label>
            <div class="readonly-field">
              {{ formatDate(order.creationTime) }}
            </div>
          </div>

          <div class="form-group">
            <label>Customer ID</label>
            <div class="readonly-field">
              {{ order.customerId || 'N/A' }}
            </div>
          </div>

          <div class="form-group">
            <label>Mark to Shipping At</label>
            <div class="readonly-field">
              {{ order.markToShippingAt ? formatDate(order.markToShippingAt) : 'Not marked' }}
            </div>
          </div>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="form-actions">
        <button 
          @click="saveChanges" 
          :disabled="saving || !hasChanges" 
          class="btn btn-primary"
        >
          <span v-if="saving">Saving...</span>
          <span v-else>Save Changes</span>
        </button>
        
        <button 
          @click="resetForm" 
          :disabled="saving" 
          class="btn btn-secondary"
        >
          Reset
        </button>
        
        <button 
          @click="goBack" 
          :disabled="saving" 
          class="btn btn-outline"
        >
          Cancel
        </button>
      </div>

      <!-- Success Message -->
      <div v-if="successMessage" class="success-message">
        {{ successMessage }}
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import authService from '../services/authService.js';

export default {
  name: 'EditOrderView',
  data() {
    return {
      orderId: null,
      order: null,
      editedOrder: {},
      loading: true,
      saving: false,
      error: null,
      successMessage: '',
      validationErrors: {}
    };
  },
  computed: {
    hasChanges() {
      if (!this.order) return false;
      
      return (
        this.editedOrder.productName !== this.order.productName ||
        this.editedOrder.shippingAddress !== this.order.shippingAddress ||
        this.editedOrder.quantity !== this.order.quantity ||
        this.editedOrder.value !== this.order.value ||
        this.editedOrder.status !== this.order.status ||
        this.editedOrder.paymentMethod !== this.order.paymentMethod
      );
    }
  },
  created() {
    // Check if admin
    if (!authService.isAdmin()) {
      console.log('Non-admin accessing EditOrderView, redirecting');
      this.$router.push('/user-orders');
      return;
    }
    
    this.orderId = this.$route.params.id;
    if (this.orderId) {
      this.fetchOrderDetails();
    } else {
      this.error = 'Order ID not provided';
      this.loading = false;
    }
  },
  methods: {
    async fetchOrderDetails() {
      this.loading = true;
      this.error = null;
      
      try {
        const token = localStorage.getItem('token');
        
        if (!token) {
          throw new Error('Authentication token not found');
        }
        
        console.log('Fetching order details for ID:', this.orderId);
        
        const response = await axios.get(`https://127.0.0.1:7092/api/orders/${this.orderId}`, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        });
        
        this.order = response.data.data;
        this.resetForm();
        
      } catch (err) {
        console.error('Error fetching order details:', err);
        if (err.response?.status === 404) {
          this.error = 'Order not found';
        } else {
          this.error = err.message || 'Failed to load order details. Please try again.';
        }
      } finally {
        this.loading = false;
      }
    },

    async saveChanges() {
      if (!this.validateForm()) {
        return;
      }

      this.saving = true;
      this.error = null;
      this.successMessage = '';
      
      try {
        const token = localStorage.getItem('token');
        
        if (!token) {
          throw new Error('Authentication token not found');
        }
        
        const updateData = {
          id: this.order.id,
          value: parseFloat(this.editedOrder.value),
          productName: this.editedOrder.productName,
          shippingAddress: this.editedOrder.shippingAddress,
          quantity: parseInt(this.editedOrder.quantity),
          creationTime: this.order.creationTime,
          markToShippingAt: this.order.markToShippingAt,
          status: parseInt(this.editedOrder.status),
          customerId: this.order.customerId,
          paymentMethod: parseInt(this.editedOrder.paymentMethod)
        };
        
        console.log('Updating order:', this.orderId, updateData);
        
        await axios.put(
          `https://127.0.0.1:7092/api/orders/update`, 
          updateData,
          {
            headers: {
              'Authorization': `Bearer ${token}`,
              'Content-Type': 'application/json'
            }
          }
        );
        
        // Update the local order object
        this.order = { ...this.order, ...updateData };
        this.successMessage = 'Order updated successfully!';
        
        // Clear success message after 3 seconds
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
        
      } catch (err) {
        console.error('Error updating order:', err);
        if (err.response?.status === 404) {
          this.error = 'Order not found';
        } else if (err.response?.status === 400) {
          this.error = 'Invalid order data provided';
        } else {
          this.error = err.message || 'Failed to update order. Please try again.';
        }
      } finally {
        this.saving = false;
      }
    },

    validateForm() {
      this.validationErrors = {};
      
      if (!this.editedOrder.productName || this.editedOrder.productName.trim() === '') {
        this.validationErrors.productName = 'Product name is required';
      }
      
      if (!this.editedOrder.shippingAddress || this.editedOrder.shippingAddress.trim() === '') {
        this.validationErrors.shippingAddress = 'Shipping address is required';
      }
      
      if (!this.editedOrder.quantity || this.editedOrder.quantity < 1) {
        this.validationErrors.quantity = 'Quantity must be at least 1';
      }
      
      if (!this.editedOrder.value || this.editedOrder.value <= 0) {
        this.validationErrors.value = 'Unit price must be greater than 0';
      }
      
      return Object.keys(this.validationErrors).length === 0;
    },

    resetForm() {
      if (this.order) {
        this.editedOrder = {
          productName: this.order.productName,
          shippingAddress: this.order.shippingAddress,
          quantity: this.order.quantity,
          value: this.order.value,
          status: this.order.status,
          paymentMethod: this.order.paymentMethod
        };
      }
      this.validationErrors = {};
      this.successMessage = '';
    },

    goBack() {
      this.$router.push('/all-orders');
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
    }
  }
};
</script>

<style scoped>
.edit-order-container {
  padding: 1rem;
  max-width: 1200px;
  margin: 0 auto;
}

.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #e9ecef;
}

.header-actions h1 {
  color: #333;
  margin: 0;
}

.header-buttons {
  display: flex;
  gap: 0.5rem;
}

.loading, .error {
  text-align: center;
  margin: 2rem 0;
  padding: 2rem;
  background-color: #f8f9fa;
  border-radius: 8px;
}

.error {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.edit-form-container {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
  padding: 2rem;
}

@media (max-width: 768px) {
  .form-grid {
    grid-template-columns: 1fr;
    gap: 1.5rem;
    padding: 1rem;
  }
}

.form-section {
  background-color: #f8f9fa;
  padding: 1.5rem;
  border-radius: 8px;
}

.section-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #4CAF50;
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
  border: 1px solid #ced4da;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
  resize: vertical;
}

.form-control:focus {
  border-color: #4CAF50;
  outline: 0;
  box-shadow: 0 0 0 0.2rem rgba(76, 175, 80, 0.25);
}

.form-control.error {
  border-color: #dc3545;
}

.form-control.disabled {
  background-color: #e9ecef;
  opacity: 1;
}

.error-text {
  color: #dc3545;
  font-size: 0.875rem;
  margin-top: 0.25rem;
  display: block;
}

.total-display {
  padding: 0.75rem;
  background-color: #e8f5e8;
  border: 1px solid #4CAF50;
  border-radius: 4px;
  font-weight: 600;
  color: #2e7d32;
  font-size: 1.1rem;
}

.readonly-field {
  padding: 0.75rem;
  background-color: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 4px;
  color: #6c757d;
}

.form-actions {
  padding: 1.5rem 2rem;
  background-color: #f8f9fa;
  border-top: 1px solid #dee2e6;
  display: flex;
  gap: 1rem;
  align-items: center;
}

.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.15s ease-in-out;
  text-decoration: none;
  display: inline-block;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background-color: #4CAF50;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background-color: #45a049;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background-color: #5a6268;
}

.btn-outline {
  background-color: transparent;
  color: #6c757d;
  border: 1px solid #6c757d;
}

.btn-outline:hover:not(:disabled) {
  background-color: #6c757d;
  color: white;
}

.success-message {
  margin-top: 1rem;
  padding: 1rem;
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
  border-radius: 4px;
  text-align: center;
}
</style>
