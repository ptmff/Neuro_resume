import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../pages/Home.vue'
import ResumeBuilder from '../pages/Resume.vue'
import ProfilePage from '../pages/ProfilePage.vue'
import JobAnalysis from '../pages/JobAnalysis.vue'

const routes = [
  { path: '/', component: HomePage },
  { path: '/resume', component: ResumeBuilder },
  { path: '/profile', component: ProfilePage },
  { path: '/analyse', component: JobAnalysis },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
