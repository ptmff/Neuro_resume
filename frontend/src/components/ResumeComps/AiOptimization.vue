<template>
  <div
    class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-white/10"
  >
    <h2
      class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8"
    >
      Оптимизация резюме с помощью AI
    </h2>
    <div
      class="ai-optimization-container p-6 rounded-2xl bg-gradient-to-br from-[var(--neon-purple)]/10 to-[var(--neon-blue)]/10"
    >
      <div class="flex items-center mb-6">
        <div
          class="ai-icon mr-4 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] p-3 rounded-full"
        >
          <i class="fas fa-robot text-white text-2xl"></i>
        </div>
        <div>
          <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-1">AI-анализ</h3>
          <p class="text-[var(--text-secondary)]">
            Наш алгоритм проанализировал ваше резюме и предлагает улучшения
          </p>
        </div>
      </div>

      <div class="space-y-4">
        <div
          v-for="(suggestion, index) in aiSuggestions"
          :key="index"
          class="suggestion-item p-4 rounded-xl bg-[var(--background-section)] bg-opacity-50 border border-white/10"
        >
          <h4 class="text-xl font-semibold text-[var(--text-light)] mb-2">
            {{ suggestion.title }}
          </h4>
          <p class="text-[var(--text-secondary)] mb-4">{{ suggestion.description }}</p>
          <div class="flex space-x-3">
            <button @click="$emit('apply-suggestion', index)" class="btn btn-primary">
              Применить
            </button>
            <button class="btn btn-secondary">Игнорировать</button>
          </div>
        </div>
      </div>
    </div>
    <div class="flex justify-between mt-8">
      <button @click="$emit('prev-step')" class="btn btn-secondary">Назад</button>
      <button @click="$emit('next-step')" class="btn btn-primary">Далее</button>
    </div>
  </div>
</template>

<script setup>
defineProps({
  aiSuggestions: {
    type: Array,
    required: true,
  },
})

defineEmits(['apply-suggestion', 'next-step', 'prev-step'])
</script>

<style scoped>
.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}

.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-white;
}

.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)] border border-white/10;
}
</style>
