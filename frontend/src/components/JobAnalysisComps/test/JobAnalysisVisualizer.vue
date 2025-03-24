<template>
    <div class="min-h-[60vh] w-full relative px-6 py-10 animate-fade-in">
      <!-- Стадия 1: Мгновенная реакция -->
      <LoadingPulse v-if="stage === 'loading'" />
  
      <!-- Стадия 2: Быстрые фразы анализа -->
      <FlashSteps v-if="stage === 'flash'" />
  
      <!-- Стадия 3: Постепенное отображение анализа -->
      <div v-if="stage === 'result' && result" class="space-y-8">
        <MatchSummary :data="result.summary" />
        <InsightCards :insights="result.insights" />
        <SkillLinkMap :skills="result.skills" />
        <TimelineMatch :timeline="result.timeline" />
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted, computed } from 'vue'
  import { useAppStore } from '@/stores/appStore'
  
  import LoadingPulse from './LoadingPulse.vue'
  import FlashSteps from './FlashSteps.vue'
  import MatchSummary from './MatchSummary.vue'
  import InsightCards from './InsightCards.vue'
  import SkillLinkMap from './SkillLinkMap.vue'
  import TimelineMatch from './TimelineMatch.vue'
  
  const emit = defineEmits(['done'])
  
  const appStore = useAppStore()
  const result = computed(() => appStore.analysisResult)
  
  type Stage = 'loading' | 'flash' | 'result'
  const stage = ref<Stage>('loading')
  
  onMounted(async () => {
    // Этап 1: Имитируем загрузку
    await wait(1000)
    stage.value = 'flash'
  
    // Этап 2: Flash-анимации
    await wait(1000)
    stage.value = 'result'
  
    emit('done') // уведомим родителя (например, для перехода к следующему этапу)
  })
  
  function wait(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms))
  }
  </script>
  