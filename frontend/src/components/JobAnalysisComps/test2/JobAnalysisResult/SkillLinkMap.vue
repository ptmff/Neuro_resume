<template>
    <div class="w-[300vw] flex flex-col items-center justify-center py-16 px-8 animate-fade-in">
      <h2 class="text-xl font-bold text-[var(--text-secondary)] mb-6">Карта навыков</h2>
      <div class="relative w-full max-w-6xl h-[400px] bg-[var(--background-overlay)] rounded-2xl border border-[var(--background-pale)] overflow-hidden backdrop-blur-xl">
        <!-- Сеточная подложка -->
        <div class="absolute inset-0 bg-grid-pattern opacity-10"></div>
  
        <!-- Навыки -->
        <div
          v-for="(skill, index) in skills"
          :key="index"
          class="absolute px-3 py-1 rounded-full text-sm font-medium text-[var(--text-light)] border border-[var(--neon-blue)] bg-[var(--neon-blue)]/10 backdrop-blur-md animate-fade-skill"
          :style="{
            top: `${skill.y}%`,
            left: `${skill.x}%`,
            transform: 'translate(-50%, -50%)',
          }"
        >
          {{ skill.label }}
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    skills: {
      label: string
      x: number // в % от ширины
      y: number // в % от высоты
    }[]
  }>()
  </script>
  
  <style scoped>
  @keyframes fade-in {
    from {
      opacity: 0;
      transform: translateY(10px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
  .animate-fade-in {
    animation: fade-in 0.8s ease-out;
  }
  
  @keyframes fade-skill {
    from {
      opacity: 0;
      transform: scale(0.8);
    }
    to {
      opacity: 1;
      transform: scale(1);
    }
  }
  .animate-fade-skill {
    animation: fade-skill 0.6s ease-out;
  }
  
  .bg-grid-pattern {
    background-image: linear-gradient(to right, rgba(255, 255, 255, 0.07) 1px, transparent 1px),
      linear-gradient(to bottom, rgba(255, 255, 255, 0.07) 1px, transparent 1px);
    background-size: 40px 40px;
  }
  </style>
  