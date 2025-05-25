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
      return userData.role === '2' ? '/all-orders' : '/user-orders'
    }
  },
  {
    path: '/all-orders',
    name: 'all-orders',
    component: AllOrdersView,
    meta: { requiresAuth: true, role: '2' }
  },
  {
    path: '/user-orders',
    name: 'user-orders',
    component: UserOrdersView,
    meta: { requiresAuth: true, roles: ['1', '2'] }  // Allow both users and admins
  },
  {
    path: '/user-account',
    name: 'user-account',
    component: UserAccountView,
    meta: { requiresAuth: true } // Everyone can access their account
  },
  {
    path: '/create-order',
    name: 'create-order',
    component: CreateOrderView,
    meta: { requiresAuth: true }
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
  
  // Enhanced debugging
  console.log('Navigating to:', to.path)
  console.log('From:', from.path)
  console.log('User data:', userData)
  console.log('User role (type):', typeof userData.role, userData.role)
  
  // Case 1: Not authenticated trying to access protected route
  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    console.log('Not authenticated, redirecting to login')
    next({ path: '/login' })
    return
  } 
  
  // Case 2: Authenticated user trying to access login/register
  if ((to.path === '/login' || to.path === '/register') && isAuthenticated) {
    console.log('Already authenticated, redirecting to appropriate page')
    const redirectPath = userData.role === '2' ? '/all-orders' : '/user-orders'
    next({ path: redirectPath })
    return
  } 
  
  // Case 3: Check role-based access
  if (to.meta.requiresAuth && isAuthenticated) {
    // Admin can access everything
    if (userData.role === '2') {
      console.log('Admin access granted')
      next()
      return
    }
    
    // Check specific role requirement
    if (to.meta.role && userData.role !== to.meta.role) {
      console.log('Access denied - incorrect role')
      // Prevent redirect loop - don't redirect if already on user-orders
      if (to.path !== '/user-orders') {
        next({ path: '/user-orders' })
      } else {
        next() // Allow access to avoid loop
      }
      return
    }
    
    // Check multiple roles requirement
    if (to.meta.roles && Array.isArray(to.meta.roles) && 
        !to.meta.roles.includes(String(userData.role))) {
      console.log('Access denied - role not in allowed roles')
      // Prevent redirect loop - don't redirect if already on user-orders
      if (to.path !== '/user-orders') {
        next({ path: '/user-orders' })
      } else {
        next() // Allow access to avoid loop
      }
      return
    }
  
    // If no specific role requirements or role checks passed,
    // allow access to authenticated user
    console.log('Authenticated access granted')
    next()
    return
  }
  
  // Default: allow navigation (for public routes)
  next()
})

export default router