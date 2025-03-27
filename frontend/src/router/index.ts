import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import HomePage from '@/pages/Home.vue';
import ResumeBuilder from '@/pages/Resume.vue';
import ProfilePage from '@/pages/ProfilePage.vue';
import JobAnalysis from '@/pages/JobAnalysis.vue';
import TestJobAnalysis from '@/pages/TestJobAnalysis.vue';
import JobAnalysisJourney from '@/pages/JobAnalysisJourney.vue';
import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';

import { useAuthStore } from '@/stores/authStore';

const routes: Array<RouteRecordRaw> = [
  { path: '/', component: HomePage },
  { path: '/resume', component: ResumeBuilder, meta: { requiresAuth: true } },
  { path: '/profile', component: ProfilePage, meta: { requiresAuth: true } },
  { path: '/analyse', component: JobAnalysis, meta: { requiresAuth: true } },
  { path: '/test', component: TestJobAnalysis },
  { path: '/test2', component: JobAnalysisJourney },
  { path: '/login', component: Login },
  { path: '/register', component: Register },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const auth = useAuthStore();
  const requiresAuth = to.meta.requiresAuth as boolean;

  if (requiresAuth && !auth.token) {
    next('/login');
  } else {
    next();
  }
});

export default router;
