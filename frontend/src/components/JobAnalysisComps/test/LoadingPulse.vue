<template>
    <div class="flex flex-col items-center justify-center gap-6 text-center animate-fade-in">
      <!-- Визуальный пульс -->
      <div class="relative w-16 h-16">
        <div class="absolute inset-0 rounded-full bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] blur-xl opacity-70 animate-ping"></div>
        <div class="relative w-full h-full rounded-full bg-[var(--background-cta)] border-4 border-[var(--neon-blue)] shadow-lg"></div>
      </div>
  
      <!-- Текст -->
      <div class="space-y-1 text-[var(--text-light)]">
        <p class="text-lg font-semibold">AI подключается...</p>
        <p class="text-sm text-[var(--text-secondary)]">Анализируем вакансию, сопоставляем с вашим резюме</p>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  // никакой логики пока не нужно
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
    animation: fade-in 0.6s ease-out forwards;
  }
  </style>
  