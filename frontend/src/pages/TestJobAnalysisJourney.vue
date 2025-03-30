<script setup lang="ts">
import { ref, computed, nextTick } from 'vue'
import { useAnalysisStore } from '@/stores/test3-analysisStore'

import JobInputPanel from '@/components/JobAnalysisComps/test3/JobInputPanel.vue'
import LoadingPhases from '@/components/JobAnalysisComps/test3/LoadingPhases.vue'
import JobAnalysisResult from '@/components/JobAnalysisComps/test3/JobAnalysisResult.vue'

const analysisStore = useAnalysisStore()
const jobUrl = ref('')
const analysisStarted = ref(false)

const isComplete = computed(() =>
  analysisStarted.value &&
  analysisStore.phases.every(p => p.status === 'done' || p.status === 'skipped')
)

const start = async (url: string) => {
  jobUrl.value = url
  await nextTick()
  await analysisStore.startJobAnalysis(url)
  analysisStarted.value = true
}
</script>

<template>
  <div class="w-full min-h-screen text-white px-4 py-12 overflow-hidden">
    <div class="max-w-3xl w-full mx-auto flex items-center justify-center min-h-[calc(100vh-96px)] fade-in">

      <!-- Ввод ссылки -->
      <transition name="fade">
        <JobInputPanel v-if="!jobUrl" @start="start" />
      </transition>

      <!-- Прогрузка фаз -->
      <transition name="fade" mode="out-in">
        <LoadingPhases v-if="jobUrl && !isComplete" key="phases" />
      </transition>

      <!-- Результат -->
        <JobAnalysisResult
          v-if="isComplete"
          key="result"
          @edit="() => console.log('Улучшить резюме')"
          @restart="() => {
            analysisStore.clear()
            jobUrl = ''
            analysisStarted.value = false
          }"
        />

    </div>
  </div>
</template>

<style scoped>
.fade-enter-active, .fade-leave-active {
  transition: all 0.6s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}

.fade-in {
  animation: fadeInUp 0.7s ease both;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
