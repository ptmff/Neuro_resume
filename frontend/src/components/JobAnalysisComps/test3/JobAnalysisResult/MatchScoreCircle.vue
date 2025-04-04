<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'

const props = defineProps<{ score: number }>()

const dasharray = 282.6
const animatedScore = ref(0)

function animateTo(target: number) {
  let current = 0
  const step = () => {
    if (current < target) {
      current += 1
      animatedScore.value = current
      requestAnimationFrame(step)
    } else {
      animatedScore.value = target
    }
  }
  requestAnimationFrame(step)
}

onMounted(() => {
  // Подождать, пока AOS-анимация закончит появление (например, delay 200 + время анимации)
  setTimeout(() => {
    animateTo(props.score)
  }, 400) // 200ms delay + 200ms fade-in = 400ms
})
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
        :stroke-dasharray="dasharray"
        :stroke-dashoffset="dasharray - (dasharray * animatedScore) / 100"
        stroke-linecap="round"
        style="transition: stroke-dashoffset 0.3s ease-out"
      />
      <defs>
        <linearGradient id="gradient">
          <stop offset="0%" stop-color="var(--neon-purple)" />
          <stop offset="100%" stop-color="var(--neon-blue)" />
        </linearGradient>
      </defs>
    </svg>
    <div class="absolute inset-0 flex items-center justify-center text-3xl font-bold text-[var(--text-light)]">
      {{ animatedScore }}%
    </div>
  </div>
</template>
