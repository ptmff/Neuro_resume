<template>
  <div class="relative w-full h-screen overflow-hidden">
    <!-- 🌌 Фоновое видео -->
    <video
      :src="footage"
      ref="videoRef"
      class="absolute top-0 left-0 w-full h-full object-cover filter blur-xl rotate-180"
      muted
      playsinline
      autoplay
      loop
    >
      Ваш браузер не поддерживает видео.
    </video>

    <!-- 🧊 Статичный верхний слой -->
    <StaticIntroSection :fadeOut="scrolled" :phase="props.phase" />

    <!-- 🧭 Подсказка скролла -->
    <HintArrow :visible="!scrolled && props.phase === 'input'" />

    <!-- 🚪 Наслаивающийся слой: инпут + футер -->
    <ScrollContainer
      v-if="props.phase === 'input'"
      @scrolled="handleScroll"
      @start="startAnalysis"
    />

    <!-- ⚙️ Анимация анализа -->
    <div class="relative z-10">
      <JobAnalysisVisualizer v-if="props.phase === 'visualizing'" />
      <JobAnalysisResult v-if="props.phase === 'result'" />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import footage from '@/assets/video/footage2.mp4'
import StaticIntroSection from '@/components/JobAnalysisComps/test/StaticIntroSection.vue'
import ScrollContainer from '@/components/JobAnalysisComps/test/ScrollContainer.vue'
import JobAnalysisVisualizer from '@/components/JobAnalysisComps/JobAnalysisVisualizer.vue'
import JobAnalysisResult from '@/components/JobAnalysisComps/JobAnalysisResult.vue'

const props = defineProps({
  phase: String,
  setPhase: Function
})

const scrolled = ref(false)

function handleScroll() {
  scrolled.value = true
}

function startAnalysis(url) {
  props.setPhase('visualizing')
  setTimeout(() => {
    props.setPhase('result')
  }, 4500)
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
