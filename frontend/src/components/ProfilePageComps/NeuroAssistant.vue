<template>
  <section
    class="mt-12 bg-white/5 border border-white/10 rounded-xl p-6 flex flex-col sm:flex-row items-center gap-4"
  >
    <div
      class="w-16 h-16 rounded-full bg-gradient-to-br from-purple-500 to-indigo-500 flex items-center justify-center text-white text-2xl shadow-lg"
    >
      🤖
    </div>
    <div class="text-center sm:text-left flex-1">
      <h3 class="text-lg font-semibold text-white mb-1">Подсказка от AI</h3>
      <p class="text-sm text-white/70">
        Добавь конкретные достижения в опыт работы, чтобы привлечь внимание рекрутера.
      </p>
    </div>
    <div class="flex gap-2">
      <button
        class="px-4 py-1.5 text-sm rounded-full bg-violet-600 hover:bg-violet-700 text-white transition"
      >
        Применить
      </button>
      <button
        class="px-4 py-1.5 text-sm rounded-full bg-white/10 hover:bg-white/20 text-white transition"
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
