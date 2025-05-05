import { createRouter, createWebHistory } from 'vue-router'
import AllOrdersView from '@/views/AllOrdersView.vue'
import UserOrdersView from '@/views/UserOrdersView.vue'
import UserAccountView from '@/views/UserAccountView.vue'
import CreateOrderView from '@/views/CreateOrderView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: AllOrdersView
  },
  {
    path: '/user-orders',
    name: 'user-orders',
    component: UserOrdersView
  },
  {
    path: '/user-account',
    name: 'user-account',
    component: UserAccountView,
    meta: { requiresAuth: true }
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
  // {
  //   path: '/about',
  //   name: 'about',
  //   // route level code-splitting
  //   // this generates a separate chunk (about.[hash].js) for this route
  //   // which is lazy-loaded when the route is visited.
  //   component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  // }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('token')
  
  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    // Redirect to login if trying to access a protected route without auth
    next({ path: '/login' })
  } else if ((to.path === '/login' || to.path === '/register') && isAuthenticated) {
    // Redirect to home if already logged in
    next({ path: '/' })
  } else {
    next()
  }
})

export default router
