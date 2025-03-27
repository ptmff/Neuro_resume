<template>
  <div class="w-[210mm]">
    <div class="w-[210mm] min-h-[297mm] p-[20mm] mx-auto bg-white shadow-lg relative overflow-hidden" ref="resumeContent">
      <!-- Шапка резюме -->
      <div class="mb-4">
        <h1 class="text-3xl font-bold text-gray-800 m-0 mb-1">{{ resumeData.name || profile?.name || 'Имя Фамилия' }}</h1>
        <p class="text-lg text-gray-600 m-0 mb-4">{{ resumeData.job || profile?.profession || 'Профессия' }}</p>
        
        <div class="flex flex-wrap gap-4">
          <div v-if="resumeData.email || profile?.email" class="flex items-center gap-2 text-sm text-gray-600">
            <i class="fas fa-envelope text-[var(--neon-purple)]"></i>
            <span>{{ resumeData.email || profile?.email }}</span>
          </div>
          <div v-if="resumeData.phone || profile?.phone" class="flex items-center gap-2 text-sm text-gray-600">
            <i class="fas fa-phone text-[var(--neon-purple)]"></i>
            <span>{{ resumeData.phone || profile?.phone }}</span>
          </div>
          <div v-if="resumeData.city || profile?.city" class="flex items-center gap-2 text-sm text-gray-600">
            <i class="fas fa-map-marker-alt text-[var(--neon-purple)]"></i>
            <span>{{ resumeData.city || profile?.city }}</span>
          </div>
        </div>
      </div>
      
      <div class="h-0.5 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] my-4"></div>
      
      <!-- Основное содержимое -->
      <div class="flex flex-col gap-5">
        <!-- О себе -->
        <div class="mb-5" v-if="resumeData.description">
          <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Обо мне</h2>
          <p class="text-sm text-gray-600 m-0 leading-relaxed">{{ resumeData.description }}</p>
        </div>
        
        <!-- Опыт работы -->
        <div class="mb-5" v-if="resumeData.experience?.length">
          <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Опыт работы</h2>
          <div class="mb-4" v-for="(exp, i) in resumeData.experience" :key="i">
            <div class="flex justify-between mb-1">
              <h3 class="text-base font-semibold text-gray-800 m-0">{{ exp.position }} | {{ exp.company }}</h3>
              <span class="text-sm text-gray-500">{{ exp.startDate }} – {{ exp.endDate }}</span>
            </div>
            <p class="text-sm text-gray-600 m-0 mt-1 leading-relaxed">{{ exp.description }}</p>
          </div>
        </div>
        
        <!-- Образование -->
        <div class="mb-5" v-if="profile?.education?.length">
          <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Образование</h2>
          <div class="mb-2.5" v-for="(edu, i) in profile.education" :key="i">
            <h3 class="text-base font-semibold text-gray-800 m-0">{{ edu.institution }}</h3>
            <p class="text-sm text-gray-600 mt-0.5 m-0">{{ edu.degree }} ({{ edu.startYear }}–{{ edu.endYear }})</p>
          </div>
        </div>
        
        <!-- Навыки -->
        <div class="mb-5" v-if="resumeData.skills?.length">
          <h2 class="text-lg font-semibold text-[var(--neon-purple)] m-0 mb-4 pb-1 border-b border-gray-200">Профессиональные навыки</h2>
          <div class="flex flex-wrap gap-2.5">
            <div 
              v-for="(skill, index) in resumeData.skills" 
              :key="skill + index"
              class="px-3 py-1 border border-gray-200 rounded-full text-sm text-gray-600"
            >
              {{ skill }}
            </div>
          </div>
        </div>
      </div>
      
    </div>
    <div class="flex justify-between mt-8">
      <button @click="$emit('prev-step')" class="btn btn-secondary">Назад</button>
      <button @click="$emit('next-step')" class="btn btn-primary">Далее</button>
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

const emit = defineEmits(['next-step', 'prev-step'])
</script>

<style scoped>

.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}
.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)];
}
.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)];
}
</style>