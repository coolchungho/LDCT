import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/cases' },
    {
      path: '/login',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/cases',
      component: () => import('../views/CaseListView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/cases/:caseId/follow-up',
      component: () => import('../views/FollowUpView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/sms',
      component: () => import('../views/SmsView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/reporting',
      component: () => import('../views/ReportingView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/alerts',
      component: () => import('../views/AlertsView.vue'),
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return { path: '/login', query: { redirect: to.fullPath } }
  }
  if (to.path === '/login' && auth.isAuthenticated) {
    return { path: '/cases' }
  }
  return true
})

export default router
