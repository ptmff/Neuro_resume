<template>
    <div class="relative w-32 h-32">
      <!-- Пульсирующее свечение -->
      <div
        class="absolute inset-0 rounded-full blur-2xl opacity-60 animate-pulse-fast"
        :style="{
          background: 'radial-gradient(circle, var(--neon-blue), transparent)',
        }"
      ></div>
  
      <!-- Основное ядро мозга -->
      <div
        class="relative w-full h-full rounded-full shadow-xl"
        :style="{
          background: 'radial-gradient(circle at center, var(--neon-purple), var(--background-cta))',
          border: '3px solid var(--neon-blue)',
        }"
      ></div>
    </div>
  </template>
  
  <script setup lang="ts">
  // пока без логики
  </script>
  
  <style scoped>
  @keyframes pulse-fast {
    0%, 100% {
      transform: scale(1);
      opacity: 0.6;
    }
    50% {
      transform: scale(1.15);
      opacity: 0.95;
    }
  }
  
  .animate-pulse-fast {
    animation: pulse-fast 1.8s ease-in-out infinite;
  }
  </style>