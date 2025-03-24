<template>
    <div class="bg-[var(--background-section)] border border-[var(--background-pale)] rounded-2xl p-6 shadow-lg animate-fade-in">
      <h3 class="text-xl font-semibold text-[var(--text-light)] mb-4">Навыки и совпадения</h3>
  
      <div class="flex flex-wrap gap-3 justify-center">
        <span
          v-for="(skill, i) in skills.matched"
          :key="'match-' + i"
          class="px-3 py-1 rounded-full bg-green-600/20 border border-green-500 text-green-300 text-sm"
        >
          ✔️ {{ skill }}
        </span>
  
        <span
          v-for="(skill, i) in skills.missing"
          :key="'miss-' + i"
          class="px-3 py-1 rounded-full bg-red-600/20 border border-red-400 text-red-300 text-sm"
        >
          ❌ {{ skill }}
        </span>
  
        <span
          v-for="(skill, i) in skills.similar"
          :key="'sim-' + i"
          class="px-3 py-1 rounded-full bg-yellow-500/10 border border-yellow-300 text-yellow-200 text-sm"
        >
          ❔ {{ skill }}
        </span>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    skills: {
      matched: string[]
      missing: string[]
      similar: string[]
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
  