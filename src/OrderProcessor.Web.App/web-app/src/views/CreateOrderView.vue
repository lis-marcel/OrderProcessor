<template>
    <div class="create-order-container">
      <h1>Create New Order</h1>
      
      <!-- Progress bar -->
      <div class="progress-bar">
        <div class="progress-steps">
          <div 
            v-for="(step, index) in steps" 
            :key="index"
            :class="['step', { 'active': currentStep === index, 'completed': currentStep > index }]"
            @click="goToStep(index)"
          >
            <div class="step-number">{{ index + 1 }}</div>
            <div class="step-title">{{ step.title }}</div>
          </div>
        </div>
      </div>
      
      <!-- Step content -->
      <div class="step-content">
        <!-- Step 1: Product Selection -->
        <div v-if="currentStep === 0" class="product-selection">
          <h2>Select Product</h2>
          <div class="form-group">
            <label for="ProductName">Product Name*</label>
            <input 
              type="text" 
              id="ProductName" 
              v-model="orderData.ProductName" 
              required
              :class="{ 'error': validationErrors.ProductName }"
            >
            <div v-if="validationErrors.ProductName" class="error-message">
              {{ validationErrors.ProductName }}
            </div>
          </div>
          
          <div class="form-group">
            <label for="Quantity">Quantity*</label>
            <input 
              type="number" 
              id="Quantity" 
              v-model="orderData.Quantity" 
              min="1" 
              required
              :class="{ 'error': validationErrors.Quantity }"
            >
            <div v-if="validationErrors.Quantity" class="error-message">
              {{ validationErrors.Quantity }}
            </div>
          </div>
          
          <div class="form-group">
            <label for="Value">Price*</label>
            <input 
              type="number" 
              id="Value" 
              v-model="orderData.Value" 
              min="0.01" 
              step="0.01" 
              required
              :class="{ 'error': validationErrors.value }"
            >
            <div v-if="validationErrors.value" class="error-message">
              {{ validationErrors.value }}
            </div>
          </div>
        </div>
        
        <!-- Step 2: Shipping Information -->
        <div v-if="currentStep === 1" class="shipping-info">
          <h2>Shipping Information</h2>
          <div class="form-group">
            <label for="ShippingAddress">Shipping Address*</label>
            <textarea 
              id="ShippingAddress" 
              v-model="orderData.ShippingAddress" 
              rows="4" 
              required
              :class="{ 'error': validationErrors.ShippingAddress }"
            ></textarea>
            <div v-if="validationErrors.ShippingAddress" class="error-message">
              {{ validationErrors.ShippingAddress }}
            </div>
          </div>
        </div>
        
        <!-- Step 3: Customer Information -->
        <div v-if="currentStep === 2" class="customer-info">
          <h2>Customer Information</h2>
          <div class="form-group">
            <label for="CustomerId">Customer ID*</label>
            <input 
              type="number" 
              id="CustomerId" 
              v-model="orderData.CustomerId" 
              min="1" 
              required
              :class="{ 'error': validationErrors.CustomerId }"
            >
            <div v-if="validationErrors.CustomerId" class="error-message">
              {{ validationErrors.CustomerId }}
            </div>
          </div>
        </div>
        
        <!-- Step 4: Payment Information -->
        <div v-if="currentStep === 3" class="payment-info">
          <h2>Payment Method</h2>
          <div class="form-group">
            <label for="PaymentMethod">Select Payment Method*</label>
            <select 
              id="PaymentMethod" 
              v-model.number="orderData.PaymentMethod" 
              required
              :class="{ 'error': validationErrors.PaymentMethod }"
            >
              <option value="">Select Payment Method</option>
              <option :value="1">Cash on Delivery</option>
              <option :value="2">Credit Card</option>
            </select>
            <div v-if="validationErrors.PaymentMethod" class="error-message">
              {{ validationErrors.PaymentMethod }}
            </div>
          </div>
        </div>
        
        <!-- Step 5: Order Review -->
        <div v-if="currentStep === 4" class="order-review">
          <h2>Review Your Order</h2>
          
          <div class="review-section">
            <h3>Product Details</h3>
            <p><strong>Product Name:</strong> {{ orderData.ProductName }}</p>
            <p><strong>Quantity:</strong> {{ orderData.Quantity }}</p>
            <p><strong>Price:</strong> {{ formatCurrency(orderData.Value) }}</p>
            <p><strong>Total:</strong> {{ formatCurrency(orderData.Value * orderData.Quantity) }}</p>
          </div>
          
          <div class="review-section">
            <h3>Shipping Information</h3>
            <p>{{ orderData.ShippingAddress }}</p>
          </div>
          
          <div class="review-section">
            <h3>Customer Information</h3>
            <p><strong>Customer ID:</strong> {{ orderData.CustomerId }}</p>
          </div>
          
          <div class="review-section">
            <h3>Payment Method</h3>
            <p>{{ getPaymentMethodText(orderData.PaymentMethod) }}</p>
          </div>
        </div>
        
        <!-- Navigation buttons -->
        <div class="form-navigation">
          <button 
            v-if="currentStep > 0" 
            @click="prevStep" 
            class="btn btn-secondary"
          >
            Previous
          </button>
          
          <button 
            v-if="currentStep < steps.length - 1" 
            @click="nextStep" 
            class="btn btn-primary"
          >
            Next
          </button>
          
          <button 
            v-if="currentStep === steps.length - 1" 
            @click="submitOrder" 
            class="btn btn-success"
            :disabled="submitting"
          >
            {{ submitting ? 'Submitting...' : 'Place Order' }}
          </button>
        </div>
      </div>
      
      <!-- Success message -->
      <div v-if="orderSubmitted" class="success-message">
        <h2>Order Created Successfully!</h2>
        <p>Your order has been submitted.</p>
        <div class="success-actions">
          <router-link to="/user-orders" class="btn btn-primary">Back to Orders</router-link>
          <button @click="resetForm" class="btn btn-secondary">Create Another Order</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'CreateOrderView',
    data() {
      return {
        steps: [
          { title: 'Product' },
          { title: 'Shipping' },
          { title: 'Customer' },
          { title: 'Payment' },
          { title: 'Review' }
        ],
        currentStep: 0,
        orderData: {
          Value: 0,
          ProductName: '',
          ShippingAddress: '',
          Quantity: 1,
          CustomerId: '',
          PaymentMethod: null,
        },
        validationErrors: {},
        submitting: false,
        orderSubmitted: false
      };
    },
    methods: {
      nextStep() {
        if (this.validateCurrentStep()) {
          if (this.currentStep < this.steps.length - 1) {
            this.currentStep++;
            window.scrollTo(0, 0);
          }
        }
      },
      
      prevStep() {
        if (this.currentStep > 0) {
          this.currentStep--;
          window.scrollTo(0, 0);
        }
      },
      
      goToStep(index) {
        // Only allow going to a previous step or next step if current is valid
        if (index < this.currentStep || (index === this.currentStep + 1 && this.validateCurrentStep())) {
          this.currentStep = index;
          window.scrollTo(0, 0);
        }
      },
      
      validateCurrentStep() {
        this.validationErrors = {};
        let isValid = true;
        
        // Step 1: Product validation
        if (this.currentStep === 0) {
          if (!this.orderData.ProductName) {
            this.validationErrors.ProductName = 'Product name is required';
            isValid = false;
          }
          
          if (!this.orderData.Quantity || this.orderData.Quantity < 1) {
            this.validationErrors.Quantity = 'Quantity must be at least 1';
            isValid = false;
          }
          
          if (!this.orderData.Value || this.orderData.Value <= 0) {
            this.validationErrors.value = 'Price must be greater than 0';
            isValid = false;
          }
        }
        
        // Step 2: Shipping validation
        if (this.currentStep === 1) {
          if (!this.orderData.ShippingAddress) {
            this.validationErrors.ShippingAddress = 'Shipping address is required';
            isValid = false;
          }
        }
        
        // Step 3: Customer validation
        if (this.currentStep === 2) {
          if (!this.orderData.CustomerId) {
            this.validationErrors.CustomerId = 'Customer ID is required';
            isValid = false;
          }
        }
        
        // Step 4: Payment validation
        if (this.currentStep === 3) {
          if (this.orderData.PaymentMethod === null || this.orderData.PaymentMethod === undefined || this.orderData.PaymentMethod === '') {
            this.validationErrors.PaymentMethod = 'Payment method is required';
            isValid = false;
          }
        }
        
        return isValid;
      },
      
      async submitOrder() {
        if (!this.validateCurrentStep()) {
          return;
        }
        
        this.submitting = true;
        
        try {
          const token = localStorage.getItem('token');

          await axios.post('https://127.0.0.1:7092/api/orders/create', 
          {
            Value: parseFloat(this.orderData.Value),
            ProductName: this.orderData.ProductName,
            ShippingAddress: this.orderData.ShippingAddress,
            Quantity: parseInt(this.orderData.Quantity),
            CustomerId: parseInt(this.orderData.CustomerId),
            PaymentMethod: parseInt(this.orderData.PaymentMethod),
          },
          {
            headers: {
              'Authorization': `Bearer ${token}`,
              'Content-Type': 'application/json'
            }
          }
        );
          this.submitting = false;
          this.orderSubmitted = true;
        } catch (error) {
          console.error('Error creating order:', error);
          this.submitting = false;
          console.error('Failed to create order. Please try again.');
        }
      },
      
      resetForm() {
        this.orderData = {
          ProductName: '',
          Value: 0,
          Quantity: 1,
          ShippingAddress: '',
          CustomerId: '',
          PaymentMethod: null,
          creationTime: new Date().toISOString()
        };
        this.currentStep = 0;
        this.validationErrors = {};
        this.orderSubmitted = false;
      },
      
      formatCurrency(Value) {
        return new Intl.NumberFormat('pl-PL', { 
          style: 'currency', 
          currency: 'PLN' 
        }).format(Value);
      },
      
      getPaymentMethodText(methodCode) {
        // Map payment method codes to human-readable text
        const methods = {
          1: 'Cash on Delivery',
          2: 'Credit Card',
        };
        return methods[methodCode] || `Unknown (${methodCode})`;
      }
    }
  };
  </script>
  
  <style scoped>
  .create-order-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 2rem;
  }
  
  h1 {
    text-align: center;
    margin-bottom: 2rem;
  }
  
  .progress-bar {
    margin: 2rem 0;
  }
  
  .progress-steps {
    display: flex;
    justify-content: space-between;
    position: relative;
  }
  
  .progress-steps::before {
    content: '';
    position: absolute;
    top: 15px;
    left: 0;
    width: 100%;
    height: 2px;
    background-color: #ddd;
    z-index: -1;
  }
  
  .step {
    display: flex;
    flex-direction: column;
    align-items: center;
    cursor: pointer;
  }
  
  .step-number {
    background-color: #ddd;
    color: #555;
    width: 30px;
    height: 30px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
    font-weight: bold;
  }
  
  .step.active .step-number {
    background-color: #4CAF50;
    color: white;
  }
  
  .step.completed .step-number {
    background-color: #4CAF50;
    color: white;
  }
  
  .step-title {
    margin-top: 0.5rem;
    font-size: 0.85rem;
  }
  
  .step-content {
    background-color: #f9f9f9;
    border-radius: 8px;
    padding: 2rem;
    margin-bottom: 2rem;
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
  
  .form-group input, 
  .form-group select,
  .form-group textarea {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 1rem;
  }
  
  .form-group input.error, 
  .form-group select.error,
  .form-group textarea.error {
    border-color: #ff0000;
  }
  
  .error-message {
    color: #ff0000;
    font-size: 0.85rem;
    margin-top: 0.25rem;
  }
  
  .form-navigation {
    display: flex;
    justify-content: space-between;
    margin-top: 2rem;
  }
  
  .btn {
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
  
  .btn-secondary {
    background-color: #95a5a6;
    color: white;
  }
  
  .btn-success {
    background-color: #2ecc71;
    color: white;
  }
  
  .btn:disabled {
    opacity: 0.7;
    cursor: not-allowed;
  }
  
  .review-section {
    margin-bottom: 2rem;
    border-bottom: 1px solid #eee;
    padding-bottom: 1rem;
  }
  
  .review-section:last-child {
    border-bottom: none;
  }
  
  .success-message {
    background-color: #d4edda;
    color: #155724;
    text-align: center;
    padding: 2rem;
    border-radius: 8px;
    margin-top: 2rem;
  }
  
  .success-actions {
    display: flex;
    justify-content: center;
    gap: 1rem;
    margin-top: 1.5rem;
  }
  
  /* Responsive adjustments */
  @media (max-width: 768px) {
    .step-title {
      display: none;
    }
    
    .form-navigation {
      flex-direction: column;
      gap: 1rem;
    }
    
    .btn {
      width: 100%;
    }
  }
  </style>