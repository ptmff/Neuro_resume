<template>
    <transition-group name="flash" tag="div" class="flex flex-col items-center gap-4 text-center">
      <div
        v-for="(step, index) in visibleSteps"
        :key="index"
        class="px-6 py-3 bg-[var(--background-section)] text-[var(--text-light)] rounded-xl border border-[var(--background-pale)] shadow-md animate-pop"
      >
        {{ step }}
      </div>
    </transition-group>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue'
  
  const allSteps = [
    '✔️ Совпадения по навыкам найдены',
    '🧠 Перекрытия в опыте обнаружены',
    '📌 Отсутствующие элементы выявлены'
  ]
  
  const visibleSteps = ref<string[]>([])
  
  onMounted(() => {
    let i = 0
    const interval = setInterval(() => {
      if (i >= allSteps.length) {
        clearInterval(interval)
      } else {
        visibleSteps.value.push(allSteps[i])
        i++
      }
    }, 600) // каждые 600мс показываем следующий
  })
  </script>
  
  <style scoped>
  @keyframes pop {
    0% {
      opacity: 0;
      transform: scale(0.95) translateY(10px);
    }
    100% {
      opacity: 1;
      transform: scale(1) translateY(0);
    }
  }
  
  .animate-pop {
    animation: pop 0.4s ease-out forwards;
  }
  
  .flash-enter-active,
  .flash-leave-active {
    transition: all 0.4s ease;
  }
  .flash-enter-from,
  .flash-leave-to {
    opacity: 0;
    transform: translateY(10px);
  }
  </style>
  