import { createRouter, createWebHistory } from 'vue-router'
import AllOrdersView from '@/views/AllOrdersView.vue'
import UserOrdersView from '@/views/UserOrdersView.vue'
import UserAccountView from '@/views/UserAccountView.vue'

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
    component: UserAccountView
  },
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

export default router
