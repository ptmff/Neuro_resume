<template>
  <div class="grid grid-cols-1 sm:grid-cols-2 gap-6 mt-8">
    <div
      v-for="(card, index) in insights"
      :key="index"
      class="bg-[var(--background-overlay)] border border-[var(--highlight)] p-5 rounded-xl hover:shadow-lg transition duration-300 backdrop-blur-md"
    >
      <h3 class="text-lg font-semibold text-[var(--text-light)] mb-2">{{ card.title }}</h3>
      <p class="text-sm text-[var(--text-mainless)]">{{ card.description }}</p>
    </div>
  </div>
</template>

<script setup>
const insights = [
  {
    title: 'Добавьте достижения',
    description:
      'Укажите конкретные цифры: увеличение продаж на 30%, снижение издержек на 20% и т.д.',
  },
  {
    title: 'Включите ключевые слова из вакансии',
    description:
      'Такие слова, как "инициативность", "ответственность" и "взаимодействие с командой" важны.',
  },
  {
    title: 'Разнообразьте soft skills',
    description: 'Укажите навыки коммуникации, критического мышления и адаптивности.',
  },
  {
    title: 'Используйте глаголы действия',
    description: 'Например: "оптимизировал", "внедрил", "улучшил" вместо общих фраз.',
  },
]
</script>

<style scoped></style>
