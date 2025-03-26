<template>
  <Transition name="fade" mode="out-in" appear>
    <div class="w-full max-w-xl relative top-1/2 animate-fade-up">
      <h2 class="text-center text-[var(--text-light)] text-2xl sm:text-3xl font-bold mb-8">
        Вставь ссылку на вакансию
      </h2>

      <div class="relative">
        <input
          v-model="jobInput"
          ref="inputRef"
          type="url"
          placeholder="Вперёд за своим бу..."
          @keydown.enter="analyze"
          :class="[
            'w-full text-[var(--text-light)] rounded-xl py-4 pl-5 pr-14 text-lg transition focus:outline-none bg-[var(--background-job-analysis)]',
            isValidUrl ? (isSending ? 'led-border fast' : 'led-border') : 'error-border'
          ]"
          required
        />

        <!-- Кнопка отправки -->
        <button
          v-if="isValidUrl && !loading"
          @click="analyze"
          class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white p-3 rounded-lg hover:scale-105 active:scale-95 transition"
        >
          <i class="fas fa-arrow-right text-[var(--text-light)]"></i>
        </button>

        <!-- Вращающийся индикатор -->
        <div v-if="loading" class="absolute top-1/2 right-2 -translate-y-1/2">
          <div class="spinner"></div>
        </div>
      </div>

      <p
        class="text-sm mt-4 text-center"
        :class="isValidUrl ? 'text-[var(--text-mainless)]' : 'text-red-400'"
      >
        {{
          isValidUrl
            ? 'Пример: https://hh.ru/vacancy/123456'
            : 'Введите корректную ссылку, начинающуюся с https://'
        }}
      </p>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { ref, watch, nextTick, onMounted, computed } from 'vue'
import { useAppStore } from '@/stores/appStore'

const emit = defineEmits<{
  (e: 'start', url: string): void
}>()

const appStore = useAppStore()
const jobInput = ref('')
const loading = ref(false)
const isSending = ref(false)
const inputRef = ref<HTMLInputElement | null>(null)

const isValidUrl = computed(() => {
  try {
    const url = new URL(jobInput.value.trim())
    return ['https:', 'http:'].includes(url.protocol)
  } catch {
    return false
  }
})

onMounted(() => {
  nextTick(() => inputRef.value?.focus())
})

watch(jobInput, (val) => {
  if (
    val.length > 3 &&
    !val.startsWith('https://') &&
    !val.startsWith('http://')
  ) {
    jobInput.value = 'https://' + val
  }
})

const analyze = async () => {
  if (!isValidUrl.value || jobInput.value.length < 10) return

  loading.value = true
  isSending.value = true

  try {
    await appStore.startJobAnalysis(jobInput.value.trim())
    emit('start', jobInput.value.trim())
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
    setTimeout(() => {
      isSending.value = false
    }, 1000)
  }
}
</script>

<style scoped>
.led-border {
  border: 2px solid transparent;
  background:
    linear-gradient(var(--background-job-analysis), var(--background-job-analysis)) padding-box,
    linear-gradient(90deg, var(--neon-purple), var(--neon-blue), var(--neon-purple)) border-box;
  background-size: 200% 100%;
  background-position: 0% 0%;
  animation: ledFlow 3s linear infinite;
}
.led-border.fast {
  animation: ledFlow 0.8s linear infinite;
}

@keyframes ledFlow {
  0% {
    background-position: 0% 0%;
  }
  100% {
    background-position: 200% 0%;
  }
}

.error-border {
  border: 2px solid var(--resume-red);
  animation: errorPulse 1.2s infinite;
}

@keyframes errorPulse {
  0%, 100% {
    box-shadow: 0 0 0px var(--resume-red);
  }
  50% {
    box-shadow: 0 0 10px var(--resume-red-hover);
  }
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid transparent;
  border-top: 3px solid var(--neon-purple);
  border-right: 3px solid var(--neon-blue);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
