<template>
    <div class="bg-[var(--background-section)] border border-[var(--background-pale)] rounded-2xl p-6 shadow-lg animate-fade-in">
      <!-- Заголовок -->
      <h3 class="text-xl font-semibold text-[var(--text-light)] mb-4">Общий результат анализа</h3>
  
      <!-- Общий % -->
      <div class="text-5xl font-bold text-center text-gradient mb-6">
        {{ data.match }}%
      </div>
  
      <!-- Метрики -->
      <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <ProgressBar title="Навыки" :value="data.skills" />
        <ProgressBar title="Опыт" :value="data.experience" />
        <ProgressBar title="Ключевые слова" :value="data.keywords" />
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import ProgressBar from './subcomponents/ProgressBar.vue'
  
  defineProps<{
    data: {
      match: number
      skills: number
      experience: number
      keywords: number
    }
  }>()
  </script>
  
  <style scoped>
  .text-gradient {
    background: linear-gradient(to right, var(--neon-purple), var(--neon-blue));
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
  }
  
  .animate-fade-in {
    animation: fadeIn 0.5s ease forwards;
  }
  @keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to   { opacity: 1; transform: translateY(0); }
  }
  </style>
  