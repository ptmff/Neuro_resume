<template>
  <div class="relative w-full h-[300px] sm:h-[400px] md:h-[500px] overflow-hidden">
    <span
      v-for="(word, index) in words"
      :key="index"
      :style="getStyle(word)"
      class="absolute font-semibold text-white whitespace-nowrap select-none pointer-events-none transition-transform duration-500"
    >
      {{ word.text }}
    </span>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const rawWords = [
  'Vue.js',
  'Teamwork',
  'Leadership',
  'TypeScript',
  'Tailwind',
  'Agile',
  'API',
  'Problem-solving',
  'Frontend',
  'Scrum',
  'Figma',
  'Unit Testing',
  'Soft Skills',
  'Communication',
  'Animations',
  'Refactoring',
  'GSAP',
  'Design',
  'HTML',
  'CSS',
]

const words = ref([])

const getRandom = (min, max) => Math.random() * (max - min) + min

const generateWords = () => {
  words.value = rawWords.map(text => ({
    text,
    top: getRandom(0, 90),
    left: getRandom(0, 90),
    fontSize: getRandom(14, 28),
    rotation: getRandom(-20, 20),
  }))
}

const getStyle = word => ({
  top: `${word.top}%`,
  left: `${word.left}%`,
  fontSize: `${word.fontSize}px`,
  transform: `rotate(${word.rotation}deg)`,
  opacity: 0.75 + Math.random() * 0.25,
})

onMounted(generateWords)
</script>

<style scoped>
span {
  will-change: transform;
}
</style>
