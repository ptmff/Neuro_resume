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
  color: black;
}
</style>