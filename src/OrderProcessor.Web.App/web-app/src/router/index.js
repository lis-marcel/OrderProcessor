import { createRouter, createWebHistory } from 'vue-router'
import AllOrdersView from '@/views/AllOrdersView.vue'
import UserOrdersView from '@/views/UserOrdersView.vue'
import UserAccountView from '@/views/UserAccountView.vue'
import CreateOrderView from '@/views/CreateOrderView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import authService from '../services/authService.js'

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
    path: '/login',
    name: 'Login',
    component: LoginView
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterView
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  const isAuthenticated = authService.isAuthenticated()
  const userData = authService.getCurrentUser() || {}
  const userRole = String(userData.role || '1')
  const isAdmin = userRole === '2'
  
  // Case 1: Not authenticated trying to access protected route
  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    next({ path: '/login' })
    return
  } 
  
  // Case 2: Authenticated user trying to access login/register
  if ((to.path === '/login' || to.path === '/register') && isAuthenticated) {
    const redirectPath = isAdmin ? '/all-orders' : '/user-account'
    next({ path: redirectPath })
    return
  } 
  
  // Case 3: Check role-based access for authenticated users
  if (to.meta.requiresAuth && isAuthenticated) {
    // Admin can access everything
    if (isAdmin) {
      next()
      return
    }
    
    // Check for single role requirement
    if (to.meta.role && to.meta.role !== userRole) {
      next({ path: '/user-account' }) // Redirect regular users to their orders
      return
    }
    
    // Check for multiple roles requirement
    if (to.meta.roles && Array.isArray(to.meta.roles) && 
        !to.meta.roles.includes(userRole)) {
      next({ path: '/user-account' })
      return
    }
    
    // If no specific role requirements or role checks passed, allow access
    next()
    return
  }
  
  // Default: allow navigation (for public routes)
  next()
})

export default router