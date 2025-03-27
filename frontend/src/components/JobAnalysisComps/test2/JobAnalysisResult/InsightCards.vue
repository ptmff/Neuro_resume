<template>
    <div class="w-[300vw] flex justify-center items-center py-12 animate-fade-in">
      <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 px-12">
        <div
          v-for="(insight, index) in insights"
          :key="index"
          class="bg-[var(--background-overlay)] border border-[var(--background-pale)] rounded-xl p-5 shadow-lg hover:shadow-xl transition duration-300 backdrop-blur-xl"
        >
          <h3 class="text-lg font-semibold text-[var(--text-secondary)] mb-2">{{ insight.title }}</h3>
          <p class="text-sm text-[var(--text-main)]">{{ insight.description }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    insights: {
      title: string
      description: string
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
  </style>
  