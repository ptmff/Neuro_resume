<template>
  <div class="resume-container">
    
    
    <div class="a4-page" ref="resumeContent">
      <!-- Шапка резюме -->
      <div class="resume-header">
        <h1 class="name">{{ resumeData.name || profile?.name || 'Имя Фамилия' }}</h1>
        <p class="profession">{{ resumeData.job || profile?.profession || 'Профессия' }}</p>
        
        <div class="contact-info">
          <div v-if="resumeData.email || profile?.email" class="contact-item">
            <i class="fas fa-envelope"></i>
            <span>{{ resumeData.email || profile?.email }}</span>
          </div>
          <div v-if="resumeData.phone || profile?.phone" class="contact-item">
            <i class="fas fa-phone"></i>
            <span>{{ resumeData.phone || profile?.phone }}</span>
          </div>
          <div v-if="resumeData.city || profile?.city" class="contact-item">
            <i class="fas fa-map-marker-alt"></i>
            <span>{{ resumeData.city || profile?.city }}</span>
          </div>
        </div>
      </div>
      
      <div class="resume-divider"></div>
      
      <!-- Основное содержимое -->
      <div class="resume-body">
        <!-- О себе -->
        <div class="resume-section" v-if="resumeData.description">
          <h2 class="section-title">Обо мне</h2>
          <p class="description">{{ resumeData.description }}</p>
        </div>
        
        <!-- Опыт работы -->
        <div class="resume-section" v-if="resumeData.experience?.length">
          <h2 class="section-title">Опыт работы</h2>
          <div class="experience-item" v-for="(exp, i) in resumeData.experience" :key="i">
            <div class="experience-header">
              <h3 class="company-position">{{ exp.position }} | {{ exp.company }}</h3>
              <span class="date">{{ exp.startDate }} – {{ exp.endDate }}</span>
            </div>
            <p class="experience-description">{{ exp.description }}</p>
          </div>
        </div>
        
        <!-- Образование -->
        <div class="resume-section" v-if="profile?.education?.length">
          <h2 class="section-title">Образование</h2>
          <div class="education-item" v-for="(edu, i) in profile.education" :key="i">
            <h3 class="institution">{{ edu.institution }}</h3>
            <p class="degree">{{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})</p>
          </div>
        </div>
        
        <!-- Навыки -->
        <div class="resume-section" v-if="resumeData.skills?.length">
          <h2 class="section-title">Профессиональные навыки</h2>
          <div class="skills-container">
            <div class="skill-item" v-for="(skill, index) in resumeData.skills" :key="skill + index">
              {{ skill }}
            </div>
          </div>
        </div>
      </div>
    

    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useProfileStore } from '@/stores/profileStore'


const props = defineProps({
  resumeData: Object
})

const profileStore = useProfileStore()
const profile = computed(() => profileStore.profile)
const resumeContent = ref(null)


</script>

<style scoped>
.a4-page {
  width: 210mm;
  min-height: 297mm;
  padding: 20mm;
  margin: 0 auto;
  background: white;
  box-shadow: 0 5px 20px rgba(0,0,0,0.1);
  position: relative;
  overflow: hidden;
}

/* Шапка резюме */
.resume-header {
  margin-bottom: 15px;
}

.name {
  font-size: 28px;
  font-weight: 700;
  color: #333;
  margin: 0 0 5px 0;
}

.profession {
  font-size: 18px;
  color: #666;
  margin: 0 0 15px 0;
}

.contact-info {
  display: flex;
  flex-wrap: wrap;
  gap: 15px;
}

.contact-item {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  color: #555;
}

.contact-item i {
  color: var(--neon-purple, #9c27b0);
}

/* Разделитель */
.resume-divider {
  height: 2px;
  background: linear-gradient(90deg, var(--neon-purple, #9c27b0), var(--neon-blue, #2196f3));
  margin: 15px 0;
}

/* Основное содержимое */
.resume-body {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.resume-section {
  margin-bottom: 20px;
}

.section-title {
  font-size: 18px;
  font-weight: 600;
  color: var(--neon-purple, #9c27b0);
  margin: 0 0 15px 0;
  padding-bottom: 5px;
  border-bottom: 1px solid #eee;
}

/* Опыт работы */
.experience-item {
  margin-bottom: 15px;
}

.experience-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 5px;
}

.company-position {
  font-size: 16px;
  font-weight: 600;
  color: #333;
  margin: 0;
}

.date {
  font-size: 14px;
  color: #777;
}

.experience-description {
  font-size: 14px;
  color: #555;
  margin: 5px 0 0 0;
  line-height: 1.5;
}

/* Образование */
.education-item {
  margin-bottom: 10px;
}

.institution {
  font-size: 16px;
  font-weight: 600;
  color: #333;
  margin: 0;
}

.degree, .description {
  font-size: 14px;
  color: #555;
  margin: 3px 0 0 0;
}

/* Навыки */
.skills-container {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.skill-item {
  padding: 5px 12px;

  font-size: 14px;
  color: #555;
}

</style>