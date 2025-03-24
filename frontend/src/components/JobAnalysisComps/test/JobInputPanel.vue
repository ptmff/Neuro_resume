<template>
  <div class="w-full max-w-xl relative top-1/2">
    <h2 class="text-center text-[var(--text-light)] text-2xl sm:text-3xl font-bold mb-8">
      Вставь ссылку на вакансию
    </h2>

    <div class="relative">
      <input
        v-model="jobInput"
        type="url"
        placeholder="Вперёд за своим бу..."
        @keydown.enter="analyze"
        class="w-full bg-[var(--background-job-analysis)] text-[var(--text-light)] rounded-xl py-4 pl-5 pr-14 text-lg border border-[var(--background-pale)] focus:outline-none focus:ring-2 focus:ring-[var(--background-cta)] transition"
        required
      />

      <!-- Появляется, когда есть текст -->
      <button
        v-if="jobInput.trim().length > 10 && !loading"
        @click="analyze"
        class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-violet-500 to-indigo-500 text-white p-3 rounded-lg hover:scale-105 active:scale-95 transition"
      >
        <i class="fas fa-arrow-right text-[var(--text-light)]"></i>
      </button>

      <!-- Если идёт загрузка -->
      <div v-if="loading" class="absolute top-1/2 right-2 -translate-y-1/2 animate-pulse text-[var(--text-light)]">
        🔍
      </div>
    </div>

    <p class="text-[var(--text-mainless)] text-sm mt-4 text-center">
      Пример: https://hh.ru/vacancy/123456
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAppStore } from '@/stores/appStore'

const emit = defineEmits<{
  (e: 'start', url: string): void
}>()

const router = useRouter()
const appStore = useAppStore()

const jobInput = ref('')
const loading = ref(false)

const analyze = async () => {
  if (jobInput.value.length < 10) return

  loading.value = true

  try {
    await appStore.startJobAnalysis(jobInput.value)
    emit('start', jobInput.value.trim())
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}
</script>
