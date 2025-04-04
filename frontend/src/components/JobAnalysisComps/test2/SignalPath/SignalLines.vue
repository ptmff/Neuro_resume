<template>
  <svg
    class="absolute left-0 top-1/2 -translate-y-1/2 w-[400vw] h-[100vh] pointer-events-none"
    viewBox="0 0 4000 1000"
    preserveAspectRatio="none"
  >
    <defs>
      <linearGradient id="signalGradient" x1="0" y1="0" x2="1" y2="0">
        <stop offset="0%" stop-color="var(--neon-purple)" />
        <stop offset="100%" stop-color="var(--neon-blue)" />
      </linearGradient>

      <filter id="glow">
        <feGaussianBlur stdDeviation="4" result="coloredBlur" />
        <feMerge>
          <feMergeNode in="coloredBlur" />
          <feMergeNode in="SourceGraphic" />
        </feMerge>
      </filter>
    </defs>

    <!-- Основной горизонтальный путь -->
    <path
      d="M 100 500 H 3900"
      stroke="url(#signalGradient)"
      stroke-width="3"
      stroke-linecap="round"
      fill="none"
      filter="url(#glow)"
    />

    <!-- Вертикальные ответвления вверх и вниз -->
    <path
      v-for="(offset, index) in branchOffsets"
      :key="index + '-top'"
      :d="`M ${offset} 500 V 400`"
      stroke="url(#signalGradient)"
      stroke-width="2"
      stroke-linecap="round"
      fill="none"
      filter="url(#glow)"
    />
    <path
      v-for="(offset, index) in branchOffsets"
      :key="index + '-bottom'"
      :d="`M ${offset} 500 V 600`"
      stroke="url(#signalGradient)"
      stroke-width="2"
      stroke-linecap="round"
      fill="none"
      filter="url(#glow)"
    />
  </svg>
</template>

<script setup lang="ts">
const branchOffsets = [600, 1000, 1500, 2000, 2400, 2800, 3200] // позиции для ответвлений
</script>