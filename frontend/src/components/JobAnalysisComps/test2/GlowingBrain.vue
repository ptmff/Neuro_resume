<template>
    <div class="glowing-brain absolute top-1/2 left-[200vw] -translate-y-1/2 w-[300px] h-[300px] z-20">
      <!-- Пульсирующая сфера -->
      <div class="relative w-full h-full">
        <!-- Энергетическое свечение -->
        <div
          class="absolute inset-0 rounded-full bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] blur-3xl opacity-60 animate-pulse-slow"
        ></div>
  
        <!-- Ядро мозга AI -->
        <AiPulseCore />
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import AiPulseCore from './GlowingBrain/AiPulseCore.vue'
  </script>
  
  <style scoped>
  @keyframes pulse-slow {
    0%, 100% {
      opacity: 0.6;
      transform: scale(1);
    }
    50% {
      opacity: 0.95;
      transform: scale(1.2);
    }
  }
  
  .animate-pulse-slow {
    animation: pulse-slow 4s ease-in-out infinite;
  }
  </style>
  