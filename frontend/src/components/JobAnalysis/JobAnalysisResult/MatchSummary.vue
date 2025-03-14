<template>
    <div class="bg-white/5 border border-white/10 rounded-2xl p-6 w-full max-w-xl mx-auto text-center">
      <h2 class="text-xl font-bold text-white mb-4">Совпадение с вакансией</h2>
  
      <div class="relative w-40 h-40 mx-auto mb-4">
        <svg viewBox="0 0 100 100" class="w-full h-full">
          <circle cx="50" cy="50" r="45" stroke="white" stroke-opacity="0.1" stroke-width="10" fill="none" />
          <circle
            cx="50"
            cy="50"
            r="45"
            stroke="url(#gradient)"
            stroke-width="10"
            fill="none"
            stroke-linecap="round"
            :stroke-dasharray="circumference"
            :stroke-dashoffset="offset"
            transform="rotate(-90 50 50)"
          />
          <defs>
            <linearGradient id="gradient" x1="0" y1="0" x2="1" y2="1">
              <stop offset="0%" stop-color="#8b5cf6" />
              <stop offset="100%" stop-color="#06b6d4" />
            </linearGradient>
          </defs>
        </svg>
        <div class="absolute inset-0 flex items-center justify-center">
          <span class="text-3xl font-bold text-white">{{ matchPercent }}%</span>
        </div>
      </div>
  
      <div class="text-white/80 text-sm">
        <p><strong>{{ matchedSkills }}</strong> подходящих навыков</p>
        <p><strong>{{ missingSkills }}</strong> не хватает</p>
      </div>
    </div>
  </template>
  
  <script setup>
  import { computed } from 'vue'
  
  // Заглушка — позже данные будут приходить из анализа
  const matchPercent = 76
  const matchedSkills = 15
  const missingSkills = 5
  
  const radius = 45
  const circumference = 2 * Math.PI * radius
  const offset = computed(() => circumference * (1 - matchPercent / 100))
  </script>
  