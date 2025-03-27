import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../pages/Home.vue'
import ResumeBuilder from '../pages/Resume.vue'
import ProfilePage from '../pages/ProfilePage.vue'
import JobAnalysis from '../pages/JobAnalysis.vue'
import TestJobAnalysis from '@/pages/TestJobAnalysis.vue'
import JobAnalysisJourney from '@/pages/JobAnalysisJourney.vue'

const routes = [
  { path: '/', component: HomePage },
  { path: '/resume', component: ResumeBuilder },
  { path: '/profile', component: ProfilePage },
  { path: '/analyse', component: JobAnalysis },
  { path: '/test', component: TestJobAnalysis},
  { path: '/test2', component: JobAnalysisJourney}
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
