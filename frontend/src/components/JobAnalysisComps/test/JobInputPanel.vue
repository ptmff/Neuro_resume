<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAppStore } from '@/stores/appStore'

const router = useRouter()
const appStore = useAppStore()

const jobInput = ref('')
const loading = ref(false)

const analyze = async () => {
  if (jobInput.value.length < 10) return

  loading.value = true

  try {
    // можно также передавать resumeId, если резюме выбрано заранее
    await appStore.startJobAnalysis(jobInput.value)
    router.push('/job/visualizer') // или как у тебя настроено
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="flex flex-col items-center gap-6 p-6 max-w-2xl mx-auto animate-fade-up">
    <h1 class="text-3xl font-semibold text-center text-white">
      Проанализируй вакансию с помощью AI
    </h1>
    <textarea
      v-model="jobInput"
      placeholder="https://hh.ru/vacancy/... или вставьте описание вакансии"
      class="w-full min-h-[120px] rounded-xl bg-[#0e0e1a] text-white px-4 py-3 shadow-md focus:outline-none focus:ring-2 ring-cyan-400 transition"
    />
    <button
      @click="analyze"
      :disabled="loading || jobInput.length < 10"
      class="bg-gradient-to-r from-cyan-500 to-blue-500 text-white px-6 py-3 rounded-lg font-semibold hover:opacity-90 transition disabled:opacity-50"
    >
      <span v-if="!loading">Начать анализ</span>
      <span v-else class="animate-pulse">Анализируем...</span>
    </button>
    <p class="text-sm text-gray-400 text-center max-w-md">
      Мы не сохраняем вашу вакансию — только анализируем.
    </p>
  </div>
</template>
