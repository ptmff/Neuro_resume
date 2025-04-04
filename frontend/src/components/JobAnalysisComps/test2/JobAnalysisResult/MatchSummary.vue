<template>
    <div class="w-[300vw] h-screen absolute top-0 left-0 flex items-center justify-center">
      <div class="animate-fade-in-up text-center space-y-4">
        <div class="text-xl font-semibold text-[var(--text-light)]">
          Совпадение с вакансией
        </div>
  
        <div class="w-40 h-40 mx-auto relative">
          <svg viewBox="0 0 100 100" class="w-full h-full">
            <circle
              cx="50"
              cy="50"
              r="45"
              fill="none"
              stroke="var(--background-overlay)"
              stroke-width="10"
            />
            <circle
              cx="50"
              cy="50"
              r="45"
              fill="none"
              stroke="url(#matchGradient)"
              stroke-width="10"
              :stroke-dasharray="circumference"
              :stroke-dashoffset="dashOffset"
              stroke-linecap="round"
              transform="rotate(-90 50 50)"
            />
            <defs>
              <linearGradient id="matchGradient" x1="0" y1="0" x2="1" y2="1">
                <stop offset="0%" :stop-color="startColor" />
                <stop offset="100%" :stop-color="endColor" />
              </linearGradient>
            </defs>
            <text
              x="50"
              y="54"
              text-anchor="middle"
              fill="var(--text-light)"
              font-size="16"
              font-weight="bold"
            >
              {{ percentage }}%
            </text>
          </svg>
        </div>
  
        <p class="text-sm text-[var(--text-mainless)]">
          {{ matchCount }} совпадений<br />{{ mismatchCount }} не совпали
        </p>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { computed } from 'vue'
  import { useAppStore } from '@/stores/appStore'
  
  const appStore = useAppStore()
  const summary = computed(() => appStore.analysisResult?.summary)
  
  const percentage = computed(() => summary.value?.match ?? 0)
  const matchCount = computed(() => summary.value?.skills ?? 0)
  const mismatchCount = computed(() => summary.value
    ? Math.max(0, 100 - summary.value.match)
    : 0
  )
  
  const circumference = 2 * Math.PI * 45
  const dashOffset = computed(() => circumference * (1 - percentage.value / 100))
  
  const startColor = computed(() =>
    percentage.value >= 70 ? '#00e5ff' : percentage.value >= 40 ? '#ffc107' : '#ef5350'
  )
  const endColor = computed(() =>
    percentage.value >= 70 ? '#9d00ff' : percentage.value >= 40 ? '#ff9800' : '#f44336'
  )
  </script>
  
  <style scoped>
  @keyframes fade-in-up {
    from {
      opacity: 0;
      transform: translateY(20px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
  
  .animate-fade-in-up {
    animation: fade-in-up 0.8s ease-out both;
  }
  </style>
  