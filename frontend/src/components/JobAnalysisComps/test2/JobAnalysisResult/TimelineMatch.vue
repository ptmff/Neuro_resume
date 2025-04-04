<template>
    <div class="w-[300vw] px-12 py-14 animate-fade-in">
      <div class="relative border-l-2 border-[var(--background-pale)] pl-8">
        <div
          v-for="(item, index) in timeline"
          :key="index"
          class="mb-12 relative"
        >
          <div class="absolute -left-[17px] top-1 w-4 h-4 rounded-full bg-[var(--background-cta)] shadow-[0_0_12px_var(--background-cta)]"></div>
  
          <div class="text-[var(--text-light)]">
            <h4 class="text-md font-semibold mb-1">
              {{ item.position }} <span class="opacity-60">@ {{ item.company }}</span>
            </h4>
            <p class="text-sm text-[var(--text-subdued)] mb-1">
              {{ item.startDate }} — {{ item.endDate || 'настоящее время' }}
            </p>
            <p class="text-sm text-[var(--text-main)]">{{ item.description }}</p>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    timeline: {
      company: string
      position: string
      startDate: string
      endDate?: string
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