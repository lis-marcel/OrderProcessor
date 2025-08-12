import { createRouter, createWebHistory } from 'vue-router'
import AllOrdersView from '@/views/AllOrdersView.vue'
import UserOrdersView from '@/views/UserOrdersView.vue'
import UserAccountView from '@/views/UserAccountView.vue'
import CreateOrderView from '@/views/CreateOrderView.vue'
import EditOrderView from '@/views/EditOrderView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import authService from '../services/authService.js'
import ChangePasswordView from '@/views/ChangePasswordView.vue'

const routes = [
  {
    path: '/',
    redirect: () => {
      const userData = authService.getCurrentUser() || {}
      return userData.role === '2' ? '/all-orders' : '/user-account'
    }
  },
  {
    path: '/all-orders',
    name: 'all-orders',
    component: AllOrdersView,
    meta: { requiresAuth: true, role: '2' } // Admin only
  },
  {
    path: '/user-orders',
    name: 'user-orders',
    component: UserOrdersView,
    meta: { requiresAuth: true, roles: ['1', '2'] } // Both users and admins
  },
  {
    path: '/user-account',
    name: 'user-account',
    component: UserAccountView,
    meta: { requiresAuth: true } // All authenticated users
  },
  {
    path: '/create-order',
    name: 'create-order',
    component: CreateOrderView,
    meta: { requiresAuth: true } // All authenticated users
  },
  {
    path: '/edit-order/:id',
    name: 'edit-order',
    component: EditOrderView,
    meta: { requiresAuth: true, role: '2' } // Admin only
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginView
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterView
  },
  {
    path: '/change-password',
    name: 'ChangePassword',
    component: ChangePasswordView,
    meta: { requiresAuth: true }
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  // Debug navigation
  console.log('Route navigation:', { to: to.path, from: from.path });
  
  // Get authentication status
  const isAuthenticated = authService.isAuthenticated();
  
  // Get user role information
  const userData = authService.getCurrentUser();
  const isAdmin = authService.isAdmin();
  
  console.log('Navigation guard check:', { 
    isAuthenticated, 
    userData: userData ? {
      id: userData.id,
      email: userData.email,
      accountType: userData.accountType
    } : null,
    isAdmin
  });
  
  // Case 1: Not authenticated trying to access protected route
  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    console.log('Redirecting to login: Not authenticated for protected route');
    next({ path: '/login' });
    return;
  } 
  
  // Case 2: Authenticated user trying to access login/register
  if ((to.path === '/login' || to.path === '/register') && isAuthenticated) {
    const redirectPath = isAdmin ? '/all-orders' : '/user-orders';
    console.log(`Redirecting from auth page to ${redirectPath}`);
    next({ path: redirectPath });
    return;
  }
  
  // Case 3: Admin-only routes
  if (to.meta.role === '2' && !isAdmin && isAuthenticated) {
    console.log('Access denied: Admin-only route');
    next({ path: '/user-orders' });
    return;
  }
  
  // Default: allow navigation
  console.log('Access granted to:', to.path);
  next();
});

export default router