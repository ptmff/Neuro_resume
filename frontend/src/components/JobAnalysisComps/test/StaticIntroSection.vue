<template>
  <section
    v-if="phase === 'input'"
    class="absolute inset-0 z-10 flex flex-col items-center justify-center transition-opacity duration-700"
    :class="{ 'opacity-0 translate-y-20 pointer-events-none': fadeOut }"
  >
    <h1 class="text-4xl sm:text-6xl font-bold text-white text-center mb-6 transition-all duration-700">
      Готов к анализу своей вакансии?
    </h1>
    <p class="text-white/70 text-lg text-center max-w-xl transition-all duration-700">
      Прокрути вниз, чтобы начать. Мы поможем адаптировать твоё резюме под вакансию.
    </p>

    <div
      v-if="!fadeOut"
      class="mt-10 animate-bounce text-white/60"
    >
      <i class="fas fa-chevron-down text-2xl"></i>
    </div>
  </section>
</template>

<script setup>
defineProps({
  fadeOut: Boolean,
  phase: String
})
</script>
