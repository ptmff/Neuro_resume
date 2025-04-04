<template>
    <div class="bg-[var(--background-section)] border border-[var(--background-pale)] rounded-2xl p-6 shadow-lg space-y-4 animate-fade-in">
      <h3 class="text-xl font-semibold text-[var(--text-light)] mb-2">Рекомендации по улучшению</h3>
  
      <transition-group name="card" tag="div" class="grid grid-cols-1 sm:grid-cols-2 gap-4">
        <div
          v-for="(insight, index) in insights"
          :key="index"
          class="p-4 bg-[var(--background-job-analysis)] border border-[var(--background-pale)] rounded-xl shadow-md hover:shadow-xl transition hover:scale-[1.01]"
        >
          <p class="text-[var(--text-light)] text-sm">{{ insight.text }}</p>
        </div>
      </transition-group>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    insights: {
      text: string
    }[]
  }>()
  </script>
  
  <style scoped>
  .card-enter-active {
    transition: all 0.3s ease;
  }
  .card-enter-from {
    opacity: 0;
    transform: translateY(10px);
  }
  .card-enter-to {
    opacity: 1;
    transform: translateY(0);
  }
  
  .animate-fade-in {
    animation: fadeIn 0.5s ease forwards;
  }
  @keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to   { opacity: 1; transform: translateY(0); }
  }
  </style>
  