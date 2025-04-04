<template>
    <div class="bg-[var(--background-section)] border border-[var(--background-pale)] rounded-2xl p-6 shadow-lg animate-fade-in">
      <h3 class="text-xl font-semibold text-[var(--text-light)] mb-4">Совпадения по опыту</h3>
  
      <div class="flex flex-col gap-2 text-sm">
        <div v-for="(range, i) in timeline.matched" :key="'match-' + i" class="text-green-300">
          ✔️ Совпадает: {{ range }}
        </div>
        <div v-for="(range, i) in timeline.gap" :key="'gap-' + i" class="text-red-300">
          ❌ Пропуск: {{ range }}
        </div>
      </div>
  
      <!-- Визуальная лента -->
      <div class="mt-6 h-4 w-full bg-[var(--background-pale)] rounded-full relative overflow-hidden">
        <div
          v-for="(range, i) in timeline.matched"
          :key="'bar-match-' + i"
          class="absolute h-full bg-green-500/70 rounded-full"
          :style="{ left: `${10 + i * 20}%`, width: '15%' }"
        ></div>
        <div
          v-for="(range, i) in timeline.gap"
          :key="'bar-gap-' + i"
          class="absolute h-full bg-red-500/70 rounded-full"
          :style="{ left: `${15 + i * 25}%`, width: '10%' }"
        ></div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    timeline: {
      matched: string[]
      gap: string[]
    }
  }>()
  </script>
  
  <style scoped>
  .animate-fade-in {
    animation: fadeIn 0.5s ease forwards;
  }
  @keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to   { opacity: 1; transform: translateY(0); }
  }
  </style>
  