<template>
  <Transition name="fade" mode="out-in" appear>
    <div class="w-full max-w-xl relative animate-fade-up">
      <h2 class="text-center text-[var(--text-light)] text-2xl sm:text-3xl font-bold mb-8">
        Вставь ссылку на вакансию
      </h2>

      <div class="relative">
        <input
          v-model="jobInput"
          ref="inputRef"
          type="url"
          placeholder="Вперёд за своим бу..."
          @keydown.enter="emitUrl"
          :class="[inputClass]"
          required
        />

        <!-- Кнопка отправки -->
        <button
          v-if="isValidUrl"
          @click="emitUrl"
          class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white p-3 rounded-lg hover:scale-105 active:scale-95 transition"
        >
          <i class="fas fa-location-arrow rotate-45"></i>
        </button>
      </div>

      <!-- Подпись под полем -->
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

const emit = defineEmits<{ (e: 'start', url: string): void }>()

const jobInput = ref('')
const inputRef = ref<HTMLInputElement | null>(null)

const isValidUrl = computed(() => {
  try {
    const url = new URL(jobInput.value.trim())
    return ['https:', 'http:'].includes(url.protocol)
  } catch {
    return false
  }
})

const inputClass = computed(() =>
  [
    'w-full text-[var(--text-light)] rounded-xl py-4 pl-5 pr-14 text-lg transition focus:outline-none bg-[var(--background-job-analysis)]',
    isValidUrl.value ? 'led-border' : 'error-border'
  ].join(' ')
)

onMounted(() => nextTick(() => inputRef.value?.focus()))

watch(jobInput, (val) => {
  if (
    val.length > 3 &&
    !val.startsWith('https://') &&
    !val.startsWith('http://')
  ) {
    jobInput.value = 'https://' + val
  }
})

const emitUrl = () => {
  if (!isValidUrl.value || jobInput.value.length < 10) return
  emit('start', jobInput.value.trim())
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
</style>
