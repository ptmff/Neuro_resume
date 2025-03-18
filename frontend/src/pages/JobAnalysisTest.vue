<template>
    <div class="relative w-full h-screen overflow-hidden">

        <!-- Фоновое видео -->
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

      <!-- Статичный верхний слой -->
      <StaticIntroSection :fadeOut="scrolled" />
  
      <!-- Указатель прокрутки -->
      <HintArrow :visible="!scrolled && phase === 'input'" />
  
      <!-- Выезжающий слой (инпут + футер внутри) -->
      <ScrollContainer
        v-if="phase === 'input'"
        @scrolled="handleScroll"
        @start="startAnalysis"
      />
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue'
  import footage from '@/assets/video/footage2.mp4'
  import StaticIntroSection from '@/components/JobAnalysisComps/test/StaticIntroSection.vue'
  import ScrollContainer from '@/components/JobAnalysisComps/test/ScrollContainer.vue'
  
  const props = defineProps<{
    setPhase: (phase: 'input' | 'visualizing' | 'result') => void
    phase: 'input' | 'visualizing' | 'result'
  }>()
  
  const scrolled = ref(false)
  
  function handleScroll() {
    scrolled.value = true
  }
  
  function startAnalysis(url: string) {
    // Можно сохранить url, если нужно
    props.setPhase('visualizing')
    setTimeout(() => props.setPhase('result'), 3000)
  }
  </script>
  