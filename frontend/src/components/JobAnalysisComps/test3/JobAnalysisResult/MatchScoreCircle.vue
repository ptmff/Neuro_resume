<script setup lang="ts">
defineProps<{ score: number }>()
</script>

<template>
  <div class="relative w-40 h-40">
    <svg class="transform -rotate-90" viewBox="0 0 100 100">
      <circle
        cx="50"
        cy="50"
        r="45"
        stroke="var(--background-job-analysis)"
        stroke-width="10"
        fill="none"
      />
      <circle
        cx="50"
        cy="50"
        r="45"
        stroke="url(#gradient)"
        stroke-width="10"
        fill="none"
        :stroke-dasharray="282.6"
        :stroke-dashoffset="282.6 - (282.6 * score) / 100"
        stroke-linecap="round"
      />
      <defs>
        <linearGradient id="gradient">
          <stop offset="0%" stop-color="var(--neon-purple)" />
          <stop offset="100%" stop-color="var(--neon-blue)" />
        </linearGradient>
      </defs>
    </svg>
    <div class="absolute inset-0 flex items-center justify-center text-3xl font-bold text-[var(--text-light)]">
      {{ score }}%
    </div>
  </div>
</template>
