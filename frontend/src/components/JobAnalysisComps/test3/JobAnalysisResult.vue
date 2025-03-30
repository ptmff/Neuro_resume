<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAnalysisStore } from '@/stores/test3-analysisStore'
import MatchScoreCircle from './JobAnalysisResult/MatchScoreCircle.vue'
import SkillTag from './JobAnalysisResult/SkillTag.vue'
import RecommendationCard from './JobAnalysisResult/RecommendationCard.vue'

import {
  animateSection,
  highlightMatchedSkills,
  animateTimeline,
} from '@/composables/useAnalysisAnimations'

const store = useAnalysisStore()

// refs для анимации
const resultRef = ref<HTMLElement | null>(null)
const skillsRef = ref<HTMLElement | null>(null)
const recsRef = ref<HTMLElement | null>(null)
const actionsRef = ref<HTMLElement | null>(null)

onMounted(() => {
  animateSection('job-result')             // 🎬 Основной блок
  highlightMatchedSkills()                 // 🎯 Подсветка навыков
  animateTimeline()                        // 📈 Timeline-эффекты

  // Локально: добавить классы или стили можно тут, если надо
})
</script>

<template>
  <div
    ref="resultRef"
    data-section="job-result"
    class="w-full max-w-5xl mx-auto p-8 text-[var(--text-light)]"
  >
    <h2 class="text-3xl font-bold mb-6 text-center"
    data-aos="fade-down" data-aos-delay="100">
      Результаты анализа
    </h2>

    <!-- Match Score -->
    <div class="flex justify-center mb-10"
    data-aos="fade-down" data-aos-delay="200">
      <MatchScoreCircle :score="store.result?.matchScore || 0" />
    </div>

    <!-- Missing Skills -->
    <div
      v-if="store.result?.missingSkills?.length"
      ref="skillsRef"
      class="mb-10"
      data-section="missing-skills"
      data-aos="fade-down" data-aos-delay="300"
    >
      <h3 class="text-xl font-semibold mb-4">Не хватает навыков:</h3>
      <div class="flex flex-wrap gap-3 skill-chip">
        <SkillTag
          v-for="skill in store.result.missingSkills"
          :key="skill"
          :label="skill"
          data-aos="fade-right"
          :data-aos-delay="350+index*100"
        />
      </div>
    </div>

    <!-- Recommendations -->
    <div
      v-if="store.result?.recommendations?.length"
      ref="recsRef"
      class="mb-12"
      data-aos="fade-down" 
      data-aos-delay="450"
      data-section="recommendations"
    >
      <h3 class="text-xl font-semibold mb-4">Рекомендации AI:</h3>
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <RecommendationCard
          v-for="(rec, idx) in store.result.recommendations"
          :key="idx"
          :text="rec"
        />
      </div>
    </div>

    <!-- Action Buttons -->
    <div
      ref="actionsRef"
      class="flex justify-center gap-4 mt-10"
      data-section="result-actions"
      data-aos="fade-down" data-aos-delay="550"
    >
      <button class="btn-glow" @click="$emit('edit')">Улучшить резюме</button>
      <button class="btn-glow-outline" @click="$emit('restart')">Проанализировать снова</button>
    </div>
  </div>
</template>

<style scoped>
.btn-glow {
  @apply px-6 py-3 rounded-full text-white font-semibold bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] hover:scale-105 transition;
}
.btn-glow-outline {
  @apply px-6 py-3 rounded-full text-white font-semibold border border-[var(--neon-blue)] hover:scale-105 transition;
}
</style>
