<template>
    <div class="min-h-screen w-full flex items-center justify-center">
      <!-- Стадия 1: Ввод -->
      <JobInputPanel v-if="stage === 'input'" @start="handleStart" />
  
      <!-- Стадия 2: Визуализатор -->
      <JobAnalysisVisualizer v-if="stage === 'visualizer'" @done="handleVisualizerDone" />
  
      <!-- Стадия 3: Результат -->
      <JobAnalysisResult v-if="stage === 'result'" />
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue'
  
  import JobInputPanel from '@/components/JobAnalysisComps/test/JobInputPanel.vue'
  import JobAnalysisVisualizer from '@/components/JobAnalysisComps/test/JobAnalysisVisualizer.vue'
  
  const stage = ref<'input' | 'visualizer' | 'result'>('input')
  
  // Когда введена ссылка
  function handleStart() {
    stage.value = 'visualizer'
  }
  
  // Когда завершена визуализация
  function handleVisualizerDone() {
    stage.value = 'result'
  }
  </script>
  