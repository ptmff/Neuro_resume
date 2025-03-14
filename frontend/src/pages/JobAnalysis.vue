<template>
  <div class="relative min-h-screen overflow-hidden">
    <!-- 🎆 Фон -->
    <JobParallaxBackground />

    <!-- 💬 Центр: Ввод -->
    <section
      v-if="props.phase === 'input'"
      class="min-h-screen flex flex-col justify-center items-center relative z-10"
    >
      <JobInputPanel @start-analysis="startAnalysis" />

      <!-- 👇 Подсказка прокрутки -->
      <div
        class="absolute bottom-10 left-1/2 -translate-x-1/2 text-white/60 text-sm text-center animate-bounce"
        v-if="!showFooter"
      >
        <i class="fas fa-arrow-down text-lg mb-1 block"></i>
        <span>Прокрути вниз</span>
      </div>
    </section>

    <!-- ⚙️ Анимация анализа -->
    <div class="relative z-10">
      <JobAnalysisVisualizer v-if="props.phase === 'visualizing'" />
      <JobAnalysisResult v-if="props.phase === 'result'" />
    </div>

    <!-- 📜 Расширенный футер -->
    <ExtendedFooter v-if="triggered && props.phase === 'input'" :visible="showFooter" />
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch } from 'vue'

import JobParallaxBackground from '@/components/JobAnalysis/JobParallaxBackground.vue'
import JobInputPanel from '@/components/JobAnalysis/JobInputPanel.vue'
import JobAnalysisVisualizer from '@/components/JobAnalysis/JobAnalysisVisualizer.vue'
import JobAnalysisResult from '@/components/JobAnalysis/JobAnalysisResult.vue'
import ExtendedFooter from '@/components/JobAnalysis/ExtendedFooter.vue'

const props = defineProps({
  phase: String,
  setPhase: Function
})

const showFooter = ref(false)
const triggered = ref(false)

const handleWheel = (event) => {
  if (!triggered.value && event.deltaY > 0 && props.phase === 'input') {
    triggered.value = true
    showFooter.value = true
  }
}

onMounted(() => {
  props.setPhase('input')
  window.addEventListener('wheel', handleWheel, { passive: true })
})

onUnmounted(() => {
  window.removeEventListener('wheel', handleWheel)
})

watch(() => props.phase, (newPhase) => {
  if (newPhase === 'result' || newPhase === 'visualizing') {
    showFooter.value = false
    triggered.value = false
    window.scrollTo({ top: 0 })
  }
})

const startAnalysis = () => {
  props.setPhase('visualizing')
  setTimeout(() => {
    props.setPhase('result')
  }, 100)
}
</script>

<style scoped>
.fade-slide-enter-active {
  transition: all 0.5s ease;
}
.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(40px);
}
.fade-slide-enter-to {
  opacity: 1;
  transform: translateY(0);
}
</style>
