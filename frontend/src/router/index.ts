import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

import HomePage from '@/pages/Home.vue'
import ResumeBuilder from '@/pages/Resume.vue'
import ProfilePage from '@/pages/ProfilePage.vue'
import JobAnalysis from '@/pages/JobAnalysis.vue'
import TestJobAnalysis from '@/pages/TestJobAnalysis.vue'
import JobAnalysisJourney from '@/pages/JobAnalysisJourney.vue'
import Login from '@/pages/Login.vue'
import Register from '@/pages/Register.vue'

import { useAuthStore } from '@/stores/authStore'
import { useAppStore } from '@/stores/appStore'
import TestJobAnalysisJourney from '@/pages/TestJobAnalysisJourney.vue'

const routes: Array<RouteRecordRaw> = [
  { path: '/', component: HomePage, meta: { requiresAuth: true } },
  { path: '/resume', component: ResumeBuilder, meta: { requiresAuth: true } },
  { path: '/profile', component: ProfilePage, meta: { requiresAuth: true } },
  { path: '/analyse', component: JobAnalysis, meta: { requiresAuth: true } },
  { path: '/test', component: TestJobAnalysis },
  { path: '/test2', component: JobAnalysisJourney },
  { path: '/test3', component: TestJobAnalysisJourney },
  { path: '/login', component: Login },
  { path: '/register', component: Register }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// 🔐 Глобальный guard с инициализацией appStore
router.beforeEach(async (to, _, next) => {
  const authStore = useAuthStore()
  const appStore = useAppStore()

  const requiresAuth = to.meta.requiresAuth as boolean
  const isAuthenticated = !!authStore.token

  // 🧠 Загружаем данные, если залогинен, но ещё не инициализировали
  if (isAuthenticated && !appStore.isAppReady && !appStore.isAppLoading) {
    await appStore.initialize()
  }

  if (requiresAuth && !isAuthenticated) {
    next('/login')
  } else if ((to.path === '/login' || to.path === '/register') && isAuthenticated) {
    next('/') // или '/profile' — если хочешь редиректить туда
  } else {
    next()
  }
})

export default router
