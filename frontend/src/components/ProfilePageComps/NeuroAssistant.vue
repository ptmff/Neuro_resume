<template>
  <section
    class="mt-12 bg-[var(--background-overlay)] border border-[var(--background-pale)] rounded-xl p-6 flex flex-col sm:flex-row items-center gap-4"
  >
    <div
      class="w-16 h-16 rounded-full bg-gradient-to-br from-purple-500 to-indigo-500 flex items-center justify-center text-[var(--text-light)] text-2xl shadow-lg"
    >
      🤖
    </div>
    <div class="text-center sm:text-left flex-1">
      <h3 class="text-lg font-semibold text-[var(--text-light)] mb-1">Подсказка от AI</h3>
      <p class="text-sm text-[var(--text-mainless)]">
        Добавь конкретные достижения в опыт работы, чтобы привлечь внимание рекрутера.
      </p>
    </div>
    <div class="flex gap-2">
      <button
        class="px-4 py-1.5 text-sm rounded-full bg-[var(--background-cta)] hover:bg-[var(--background-cta-hover)] text-[var(--text-light)] transition"
      >
        Применить
      </button>
      <button
        class="px-4 py-1.5 text-sm rounded-full bg-[var(--background-pale)] hover:bg-[var(--background-indicator)] text-[var(--text-light)] transition"
      >
        Позже
      </button>
    </div>
  </section>
</template>

<script setup>
// В будущем можно подключить динамические советы от нейросети
</script>

<style scoped></style>
