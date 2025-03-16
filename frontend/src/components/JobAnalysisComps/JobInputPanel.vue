<template>
  <div class="flex items-center justify-center min-h-screen">
    <div class="w-full max-w-xl relative">
      <h2 class="text-center text-white text-2xl sm:text-3xl font-bold mb-8">
        Вставь ссылку на вакансию
      </h2>

      <div class="relative">
        <input
          v-model="link"
          type="text"
          placeholder="https://..."
          class="w-full bg-[#111111] text-white rounded-xl py-4 pl-5 pr-14 text-lg border border-white/10 focus:outline-none focus:ring-2 focus:ring-violet-600 transition"
        />

        <!-- Кнопка появляется только если есть текст -->
        <button
          v-if="link.trim()"
          @click="$emit('start-analysis')"
          class="absolute top-1/2 right-2 -translate-y-1/2 bg-gradient-to-r from-violet-500 to-indigo-500 text-white p-3 rounded-lg hover:scale-105 active:scale-95 transition"
        >
          <i class="fas fa-arrow-right text-white"></i>
        </button>
      </div>

      <!-- Подсказка -->
      <p class="text-white/50 text-sm mt-4 text-center">Пример: https://hh.ru/vacancy/123456</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

defineEmits(['start-analysis'])

const link = ref('')
</script>

<style></style>
