<template>
    <div>
      <div class="text-sm text-[var(--text-secondary)] mb-1">{{ title }}</div>
      <div class="w-full h-3 rounded-full bg-[var(--background-faded)] relative overflow-hidden">
        <div
          class="h-full rounded-full bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] transition-all duration-500"
          :style="{ width: value + '%' }"
        ></div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    title: string
    value: number
  }>()
  </script>
  