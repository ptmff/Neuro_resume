// src/router.ts
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import HomePage from '../pages/Home.vue';
import ResumeBuilder from '../pages/Resume.vue';
import ProfilePage from '../pages/ProfilePage.vue';
import JobAnalysis from '../pages/JobAnalysis.vue';
import { useAuthStore } from '@/stores/authStore';
import Login from '../pages/Login.vue'

const routes: RouteRecordRaw[] = [
  { path: '/', component: HomePage },
  { path: '/resume', component: ResumeBuilder, meta: { requiresAuth: true } },
  { path: '/profile', component: ProfilePage, meta: { requiresAuth: true } },
  { path: '/analyse', component: JobAnalysis },
  { path: '/login', component: Login }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const auth = useAuthStore();
  if (to.meta.requiresAuth && !auth.token) {
    next('/login');
  } else {
    next();
  }
});

export default router;
