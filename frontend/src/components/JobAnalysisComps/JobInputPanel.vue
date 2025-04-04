<template>
  <div class="flex items-center justify-center min-h-screen">
    <div class="w-full max-w-xl relative">
      <h2 class="text-center text-[var(--text-light)] text-2xl sm:text-3xl font-bold mb-8">
        Вставь ссылку на вакансию
      </h2>

      <div class="relative">
        <input
          v-model="link"
          type="text"
          placeholder="https://..."
          class="w-full bg-[var(--background-job-analysis)] text-[var(--text-light)] rounded-xl py-4 pl-5 pr-14 text-lg border border-[var(--background-pale)] focus:outline-none focus:ring-2 focus:ring-[var(--background-cta)] transition"
        />

        <!-- Кнопка появляется только если есть текст -->
        <button
          v-if="link.trim()"
          @click="$emit('start-analysis')"
          class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-violet-500 to-indigo-500 text-[var(--text-light)] p-3 rounded-lg hover:scale-105 active:scale-95 transition"
        >
          <i class="fas fa-arrow-right text-[var(--text-light)]"></i>
        </button>
      </div>

      <!-- Подсказка -->
      <p class="text-[var(--text-mainless)] text-sm mt-4 text-center">Пример: https://hh.ru/vacancy/123456</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

defineEmits(['start-analysis'])

const link = ref('')
</script>

<style></style>
