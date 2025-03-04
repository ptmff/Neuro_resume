import { createRouter, createWebHashHistory } from 'vue-router';
import HomePage from '../pages/HomePage.vue';
import ResumeBuilder from '../pages/ResumeBuilderPage.vue';
import AnalysePage from '../pages/AnalysePage.vue';
import CombCheckPage from '../pages/CombCheckPage.vue';
import ProfilePage from '../pages/ProfilePage.vue';

const routes = [
  { path: '/', component: HomePage },
  { path: '/resume-builder', component: ResumeBuilder },
  { path: '/analyse', component: AnalysePage },
  { path: '/comb-check', component: CombCheckPage},
  { path: '/profile', component: ProfilePage },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
