<template>
  <div class="absolute inset-0 pointer-events-none select-none overflow-hidden">
    <span
      v-for="(word, index) in floatingWords"
      :key="index"
      class="absolute text-white/80 text-sm font-medium whitespace-nowrap opacity-0 animate-float"
      :style="getStyle(word)"
    >
      {{ word.text }}
    </span>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const rawWords = [
  'Vue.js',
  'Tailwind',
  'JavaScript',
  'Teamwork',
  'Communication',
  'Leadership',
  'HTML',
  'CSS',
  'Responsiveness',
  'Agile',
  'Problem-solving',
  'Debugging',
  'Creativity',
  'Frontend',
  'UX',
]

const floatingWords = ref([])

const generateFloatingWords = () => {
  floatingWords.value = rawWords.map(text => ({
    text,
    top: Math.random() * 100,
    left: Math.random() * 100,
    delay: Math.random() * 5,
    duration: 5 + Math.random() * 5,
    direction: Math.random() > 0.5 ? 1 : -1,
  }))
}

const getStyle = word => {
  return {
    top: `${word.top}%`,
    left: `${word.left}%`,
    animationDelay: `${word.delay}s`,
    animationDuration: `${word.duration}s`,
    transform: `translateY(${word.direction * 10}px)`,
  }
}

onMounted(() => {
  generateFloatingWords()
})
</script>

<style scoped>
@keyframes float {
  0%,
  100% {
    transform: translateY(0);
    opacity: 0;
  }
  50% {
    transform: translateY(-10px);
    opacity: 1;
  }
}

.animate-float {
  animation-name: float;
  animation-timing-function: ease-in-out;
  animation-iteration-count: infinite;
}
</style>
