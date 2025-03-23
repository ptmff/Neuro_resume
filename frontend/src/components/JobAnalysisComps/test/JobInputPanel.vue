<template>
  <!-- Центрированный блок ввода -->
  <div class="w-full max-w-xl relative top-60">
    <!-- Заголовок -->
    <h2 class="text-center text-[var(--text-light)] text-2xl sm:text-3xl font-bold mb-8">
      Вставь ссылку на вакансию
    </h2>

    <!-- Поле + кнопка -->
    <div class="relative">
      <input
        v-model="url"
        type="url"
        placeholder="https://example.com/vacancy"
        class="w-full bg-[#111111] text-[var(--text-light)] rounded-xl py-4 pl-5 pr-14 text-lg border border-[var(--background-pale)] focus:outline-none focus:ring-2 focus:ring-[var(--background-cta)] transition"
        required
      />

      <button
        v-if="url.trim()"
        @click="submit"
        class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-violet-500 to-indigo-500 text-white p-3 rounded-lg hover:scale-105 active:scale-95 transition"
      >
        <i class="fas fa-arrow-right text-[var(--text-light)]"></i>
      </button>
    </div>

    <!-- Подсказка -->
    <p class="text-[var(--text-mainless)] text-sm mt-4 text-center">
      Пример: https://hh.ru/vacancy/123456
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits<{
  (e: 'start', url: string): void
}>()

const url = ref('')

function submit() {
  if (!url.value.trim()) return
  emit('start', url.value.trim())
}
</script>
