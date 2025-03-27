<template>
    <div class="relative w-screen h-screen overflow-hidden">
      <!-- Единая сцена -->
      <div
        class="absolute top-0 left-0 h-full w-[400vw] transition-transform duration-[8000ms] ease-in-out"
        :style="{ transform: `translateX(-${scrollX}vw)` }"
      >
        <!-- Этап 0: Ввод вакансии -->
        <div class="absolute top-0 left-0 w-screen h-full flex items-center justify-center">
          <JobInputPanel @start="handleStart" />
        </div>
  
        <!-- Этап 1–2: Путь сигнала и мозг AI -->
        <SignalPath class="absolute top-0 left-[100vw] w-[200vw] h-full" />
        <!-- <GlowingBrain class="absolute top-0 left-[250vw] w-[100vw] h-full" /> -->
  
        <!-- Этап 3: Финальный анализ -->
        <JobAnalysisResult class="absolute top-0 left-[300vw] w-screen h-full" />
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue'
  import JobInputPanel from '@/components/JobAnalysisComps/test2/JobInputPanel.vue'
  import SignalPath from '@/components/JobAnalysisComps/test2/SignalPath.vue'
  import GlowingBrain from '@/components/JobAnalysisComps/test2/GlowingBrain.vue'
  import JobAnalysisResult from '@/components/JobAnalysisComps/test2/JobAnalysisResult.vue'
  import { useAppStore } from '@/stores/appStore'
  
  const appStore = useAppStore()
  const scrollX = ref(0)
  
  const handleStart = async (url: string) => {
    const totalScroll = 300 // от 0 до 300vw (всего 4 экрана: 0, 100, 200, 300)
    const duration = 10000 // 10 секунд
    const steps = 100
    const stepTime = duration / steps
    const stepSize = totalScroll / steps
  
    let current = 0
  
    const interval = setInterval(() => {
      current += stepSize
      scrollX.value = current
      if (current >= totalScroll) {
        clearInterval(interval)
      }
    }, stepTime)
  
    await appStore.startJobAnalysis(url)
  }
  </script>
  
  <style scoped>
  </style>
  