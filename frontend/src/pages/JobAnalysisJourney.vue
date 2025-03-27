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
  import { useAnalysisStore } from '@/stores/analysisStore'
  import { fetchMockAnalysis } from '@/mocks/mockAnalysis'
  
  const analysisStore = useAnalysisStore()
  const scrollX = ref(0)
  
  const handleStart = async (url: string) => {
    scrollX.value = 0
  
    const totalScroll = 300
    const steps = 100
    const stepTime = 100
    const stepSize = totalScroll / steps
  
    let current = 0
  
    const interval = setInterval(() => {
      current += stepSize
      scrollX.value = current
      if (current >= totalScroll) {
        clearInterval(interval)
      }
    }, stepTime)
  
    // ⏳ Запрос на мок-данные
    const { result, steps: receivedSteps } = await fetchMockAnalysis(url)
  
    // 👣 Обновление результата
    analysisStore.result = result
    analysisStore.steps = []
  
    // 🚶 Добавляем шаги по одному
    for (const step of receivedSteps) {
      await new Promise(resolve => setTimeout(resolve, 800))
      analysisStore.steps.push(step)
    }
  }
  </script>
  
  
  <style scoped>
  </style>
  