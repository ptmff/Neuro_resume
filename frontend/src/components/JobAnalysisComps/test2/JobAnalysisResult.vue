<template>
    <div
      class="absolute left-[300vw] top-0 w-screen h-screen px-10 py-16 flex flex-col gap-10 text-[var(--text-light)] animate-fade-in"
    >
      <div class="text-center">
        <h2 class="text-3xl sm:text-4xl font-bold mb-2">Анализ завершён</h2>
        <p class="text-[var(--text-mainless)] text-sm sm:text-base">
          Это итоговая сцена. Здесь можно предложить AI-оптимизацию, редактирование и экспорт
        </p>
      </div>
  
      <MatchSummary :data="result?.summary" />
      <InsightCards :insights="mappedInsights" />
      <SkillLinkMap :skills="mappedSkills" />
      <TimelineMatch :timeline="mappedTimeline" />
    </div>
  </template>
  
  <script setup lang="ts">
  import { computed } from 'vue'
  import { useAppStore } from '@/stores/appStore'
  
  import MatchSummary from './JobAnalysisResult/MatchSummary.vue'
  import InsightCards from './JobAnalysisResult/InsightCards.vue'
  import SkillLinkMap from './JobAnalysisResult/SkillLinkMap.vue'
  import TimelineMatch from './JobAnalysisResult/TimelineMatch.vue'
  
  const appStore = useAppStore()
  const result = computed(() => appStore.analysisResult)
  
  const mappedInsights = computed(() =>
    result.value?.insights.map(i => ({
      title: 'Совет от AI',
      description: i.text
    })) ?? []
  )
  
  interface SkillItem {
    label: string;
    x: number;
    y: number;
  }
  
  const mappedSkills = computed<SkillItem[]>(() => {
    const output: SkillItem[] = []
    const offsetX = 100
    const offsetY = 60
  
    result.value?.skills.matched.forEach((s, i) => {
      output.push({ label: s, x: 100 + i * offsetX, y: 200 })
    })
    result.value?.skills.missing.forEach((s, i) => {
      output.push({ label: s, x: 100 + i * offsetX, y: 300 })
    })
    result.value?.skills.similar.forEach((s, i) => {
      output.push({ label: s, x: 100 + i * offsetX, y: 400 })
    })
  
    return output
  })
  
  interface TimelineItem {
    company: string;
    position: string;
    startDate: string;
    endDate?: string;
    description: string;
  }
  
  const mappedTimeline = computed<TimelineItem[]>(() => {
    const resultList: TimelineItem[] = []
  
    result.value?.timeline.matched.forEach((range) => {
      resultList.push({
        company: 'AI Match',
        position: 'Найдено совпадение',
        startDate: range.split('–')[0],
        endDate: range.split('–')[1],
        description: 'Опыт совпадает с вакансией'
      })
    })
  
    result.value?.timeline.gap.forEach((gap) => {
      resultList.push({
        company: 'AI Gap',
        position: 'Пробел в опыте',
        startDate: gap,
        endDate: '',
        description: 'Нет данных за этот период'
      })
    })
  
    return resultList
  })
  </script>
  
  <style scoped>
  @keyframes fade-in {
    from {
      opacity: 0;
      transform: translateY(20px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }
  
  .animate-fade-in {
    animation: fade-in 1s ease-out;
  }
  </style>
  