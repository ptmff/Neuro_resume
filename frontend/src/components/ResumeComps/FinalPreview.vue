<template>
  <div class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10">
    <h2 class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8">
      Ваше готовое резюме
    </h2>
    
    <div class="flex justify-center">
      <div class="a4-container bg-white shadow-xl">
        <!-- Содержимое резюме в зависимости от выбранного шаблона -->
        <div v-if="selectedTemplate === 0" class="classic-template">
          <div class="resume-header text-center mb-6 pt-8">
            <h1 class="text-3xl font-bold text-gray-800">{{ resumeData.name || 'Имя Фамилия' }}</h1>
            <p class="text-xl text-gray-600 mt-1">{{ resumeData.profession || 'Профессия' }}</p>
          </div>
          
          <div class="resume-contact flex justify-center space-x-4 mb-6 text-gray-700 text-sm">
            <p v-if="resumeData.email"><i class="fas fa-envelope mr-1"></i>{{ resumeData.email }}</p>
            <p v-if="resumeData.phone"><i class="fas fa-phone mr-1"></i>{{ resumeData.phone }}</p>
            <p v-if="resumeData.location"><i class="fas fa-map-marker-alt mr-1"></i>{{ resumeData.location }}</p>
          </div>
          
          <div class="resume-content px-10">
            <div class="resume-section mb-6" v-if="resumeData.education">
              <h2 class="text-xl font-semibold text-gray-800 mb-2 border-b border-gray-300 pb-1">Образование</h2>
              <p class="text-gray-700">{{ resumeData.education }}</p>
            </div>
            
            <div class="resume-section mb-6" v-if="resumeData.experience">
              <h2 class="text-xl font-semibold text-gray-800 mb-2 border-b border-gray-300 pb-1">Опыт работы</h2>
              <p class="text-gray-700">{{ resumeData.experience }}</p>
            </div>
            
            <div class="resume-section" v-if="resumeData.skills">
              <h2 class="text-xl font-semibold text-gray-800 mb-2 border-b border-gray-300 pb-1">Навыки</h2>
              <p class="text-gray-700">{{ resumeData.skills }}</p>
            </div>
          </div>
        </div>
        
        <div v-else-if="selectedTemplate === 1" class="modern-template">
          <div class="flex">
            <div class="w-1/3 bg-blue-600 text-white p-8 h-full">
              <div class="resume-header mb-8">
                <h1 class="text-2xl font-bold">{{ resumeData.name || 'Имя Фамилия' }}</h1>
                <p class="text-lg mt-1">{{ resumeData.profession || 'Профессия' }}</p>
              </div>
              
              <div class="resume-contact mb-8 text-sm">
                <p v-if="resumeData.email" class="mb-2"><i class="fas fa-envelope mr-2"></i>{{ resumeData.email }}</p>
                <p v-if="resumeData.phone" class="mb-2"><i class="fas fa-phone mr-2"></i>{{ resumeData.phone }}</p>
                <p v-if="resumeData.location"><i class="fas fa-map-marker-alt mr-2"></i>{{ resumeData.location }}</p>
              </div>
              
              <div class="resume-section" v-if="resumeData.skills">
                <h2 class="text-lg font-semibold mb-2 border-b border-white/30 pb-1">Навыки</h2>
                <p>{{ resumeData.skills }}</p>
              </div>
            </div>
            
            <div class="w-2/3 p-8">
              <div class="resume-section mb-6" v-if="resumeData.education">
                <h2 class="text-xl font-semibold text-gray-800 mb-2 border-b border-gray-300 pb-1">Образование</h2>
                <p class="text-gray-700">{{ resumeData.education }}</p>
              </div>
              
              <div class="resume-section" v-if="resumeData.experience">
                <h2 class="text-xl font-semibold text-gray-800 mb-2 border-b border-gray-300 pb-1">Опыт работы</h2>
                <p class="text-gray-700">{{ resumeData.experience }}</p>
              </div>
            </div>
          </div>
        </div>
        
        <div v-else-if="selectedTemplate === 2" class="creative-template">
          <div class="resume-header bg-gradient-to-r from-purple-600 to-pink-500 text-white p-8 text-center">
            <h1 class="text-3xl font-bold">{{ resumeData.name || 'Имя Фамилия' }}</h1>
            <p class="text-xl mt-1">{{ resumeData.profession || 'Профессия' }}</p>
            
            <div class="resume-contact flex justify-center space-x-4 mt-4 text-sm">
              <p v-if="resumeData.email"><i class="fas fa-envelope mr-1"></i>{{ resumeData.email }}</p>
              <p v-if="resumeData.phone"><i class="fas fa-phone mr-1"></i>{{ resumeData.phone }}</p>
              <p v-if="resumeData.location"><i class="fas fa-map-marker-alt mr-1"></i>{{ resumeData.location }}</p>
            </div>
          </div>
          
          <div class="resume-content p-8">
            <div class="grid grid-cols-2 gap-6">
              <div class="resume-section" v-if="resumeData.education">
                <h2 class="text-xl font-semibold text-gray-800 mb-2 pb-1 border-b-2 border-purple-500">Образование</h2>
                <p class="text-gray-700">{{ resumeData.education }}</p>
              </div>
              
              <div class="resume-section" v-if="resumeData.skills">
                <h2 class="text-xl font-semibold text-gray-800 mb-2 pb-1 border-b-2 border-pink-500">Навыки</h2>
                <p class="text-gray-700">{{ resumeData.skills }}</p>
              </div>
            </div>
            
            <div class="resume-section mt-6" v-if="resumeData.experience">
              <h2 class="text-xl font-semibold text-gray-800 mb-2 pb-1 border-b-2 border-purple-500">Опыт работы</h2>
              <p class="text-gray-700">{{ resumeData.experience }}</p>
            </div>
          </div>
        </div>
        
        <div v-else class="minimal-template p-10">
          <div class="resume-header mb-8">
            <h1 class="text-3xl font-bold text-gray-800">{{ resumeData.name || 'Имя Фамилия' }}</h1>
            <p class="text-xl text-gray-600 mt-1">{{ resumeData.profession || 'Профессия' }}</p>
            
            <div class="resume-contact flex space-x-4 mt-4 text-gray-700 text-sm">
              <p v-if="resumeData.email"><i class="fas fa-envelope mr-1"></i>{{ resumeData.email }}</p>
              <p v-if="resumeData.phone"><i class="fas fa-phone mr-1"></i>{{ resumeData.phone }}</p>
              <p v-if="resumeData.location"><i class="fas fa-map-marker-alt mr-1"></i>{{ resumeData.location }}</p>
            </div>
          </div>
          
          <div class="resume-content">
            <div class="resume-section mb-6" v-if="resumeData.education">
              <h2 class="text-lg font-semibold text-gray-800 mb-2">Образование</h2>
              <p class="text-gray-700">{{ resumeData.education }}</p>
            </div>
            
            <div class="resume-section mb-6" v-if="resumeData.experience">
              <h2 class="text-lg font-semibold text-gray-800 mb-2">Опыт работы</h2>
              <p class="text-gray-700">{{ resumeData.experience }}</p>
            </div>
            
            <div class="resume-section" v-if="resumeData.skills">
              <h2 class="text-lg font-semibold text-gray-800 mb-2">Навыки</h2>
              <p class="text-gray-700">{{ resumeData.skills }}</p>
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
defineProps({
  resumeData: {
    type: Object,
    required: true
  },
  selectedTemplate: {
    type: Number,
    required: true
  }
})

defineEmits(['prev-step', 'next-step'])
</script>

<style scoped>


.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}

.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white;
}

.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)] border border-white/10;
}
</style>