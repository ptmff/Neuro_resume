<template>
  <div
    class="bg-[var(--background-section)] bg-opacity-30 backdrop-blur-xl p-8 rounded-3xl border border-[var(--background-pale)]"
  >
    <h2
      class="text-4xl font-bold bg-gradient-to-r from-[var(--text-secondary)] to-[var(--text-light)] bg-clip-text text-transparent mb-8"
    >
      Оптимизация резюме с помощью AI
    </h2>
    
    <!-- Загрузка -->
    <div v-if="isLoading" class="ai-optimization-container p-6 rounded-2xl bg-gradient-to-br from-[var(--neon-purple)]/10 to-[var(--neon-blue)]/10 flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-16 w-16 border-t-2 border-b-2 border-[var(--neon-purple)]"></div>
    </div>
    
    <!-- Основной контент -->
    <div v-else class="ai-optimization-container p-6 rounded-2xl bg-gradient-to-br from-[var(--neon-purple)]/10 to-[var(--neon-blue)]/10">
      <div class="flex items-center mb-6">
        <div
          class="ai-icon mr-4 bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] p-3 rounded-full"
        >
          <i class="fas fa-robot text-[var(--text-light)] text-2xl"></i>
        </div>
        <div>
          <h3 class="text-2xl font-semibold text-[var(--text-light)] mb-1">AI-анализ</h3>
          <p class="text-[var(--text-secondary)]">
            Наш алгоритм проанализировал ваше резюме и предлагает улучшения
          </p>
        </div>
      </div>
      
      <!-- Статистика -->
      <div v-if="stats" class="stats-container p-4 mb-6 rounded-xl bg-[var(--background-section)] bg-opacity-70 border border-[var(--background-pale)]">
        <div class="flex justify-between items-center">
          <div>
            <h4 class="text-lg font-semibold text-[var(--text-light)]">Оценка соответствия</h4>
            <div class="flex items-center mt-2">
              <div class="w-16 text-center">
                <span class="text-lg font-bold text-[var(--text-secondary)]">{{ stats.targetPositionMatch.before }}%</span>
                <p class="text-xs text-[var(--text-mainless)]">Сейчас</p>
              </div>
              <div class="mx-2">
                <i class="fas fa-arrow-right text-[var(--text-mainless)]"></i>
              </div>
              <div class="w-16 text-center">
                <span class="text-lg font-bold text-[var(--neon-purple)]">{{ stats.targetPositionMatch.after }}%</span>
                <p class="text-xs text-[var(--text-mainless)]">После</p>
              </div>
            </div>
          </div>
          <div class="text-center">
            <div class="text-3xl font-bold text-[var(--neon-purple)]">+{{ stats.estimatedImprovementScore }}</div>
            <p class="text-xs text-[var(--text-mainless)]">Очки улучшения</p>
          </div>
          <div class="text-center">
            <div class="text-3xl font-bold text-[var(--text-light)]">{{ suggestions.length }}</div>
            <p class="text-xs text-[var(--text-mainless)]">Рекомендации</p>
          </div>
        </div>
      </div>

      <!-- Список рекомендаций -->
      <div class="space-y-4">
        <div
          v-for="suggestion in suggestions"
          :key="suggestion.id"
          class="suggestion-item p-4 rounded-xl bg-[var(--background-section)] bg-opacity-50 border border-[var(--background-pale)]"
        >
          <div class="flex justify-between items-start mb-2">
            <h4 class="text-xl font-semibold text-[var(--text-light)]">
              {{ suggestion.title }}
            </h4>
            <div class="confidence-badge px-2 py-1 rounded-full text-xs" 
                 :class="getConfidenceBadgeClass(suggestion.confidence)">
              {{ Math.round(suggestion.confidence * 100) }}% уверенность
            </div>
          </div>
          
          <p class="text-[var(--text-secondary)] mb-4">{{ suggestion.description }}</p>
          
          <div class="before-after-container mb-4 p-3 rounded-lg bg-[var(--background-section)] bg-opacity-70">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <p class="text-xs text-[var(--text-mainless)] mb-1">Сейчас:</p>
                <div class="p-2 rounded bg-[var(--background-pale)] bg-opacity-30 text-sm text-[var(--text-secondary)]">
                  {{ formatBeforeAfter(suggestion.before) }}
                </div>
              </div>
              <div>
                <p class="text-xs text-[var(--text-mainless)] mb-1">Предлагаемое изменение:</p>
                <div class="p-2 rounded bg-[var(--neon-purple)]/5 border border-[var(--neon-purple)]/10 text-sm text-[var(--text-light)]">
                  {{ formatBeforeAfter(suggestion.after) }}
                </div>
              </div>
            </div>
          </div>
          
          <div class="reasoning mb-4 text-sm text-[var(--text-mainless)] italic">
            <i class="fas fa-info-circle mr-1"></i> {{ suggestion.reasoning }}
          </div>
          
          <div class="flex space-x-3">
            <button @click="applySuggestion(suggestion.id)" class="btn btn-primary">
              <i class="fas fa-check mr-2"></i> Применить
            </button>
            <button @click="ignoreSuggestion(suggestion.id)" class="btn btn-secondary">
              <i class="fas fa-times mr-2"></i> Игнорировать
            </button>
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

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { 
  getMockAiSuggestions, 
  AiSuggestion, 
  AiSuggestionStats 
} from '@/mocks/mockAiSuggestions';

const props = defineProps({
  resumeId: {
    type: Number,
    default: 123
  },
  targetPosition: {
    type: String,
    default: 'Frontend Developer'
  },
  targetCompany: {
    type: String,
    default: 'Yandex'
  }
});

const emit = defineEmits(['apply-suggestion', 'ignore-suggestion', 'next-step', 'prev-step']);

// Состояние
const isLoading = ref(true);
const suggestions = ref<AiSuggestion[]>([]);
const stats = ref<AiSuggestionStats | null>(null);

// Методы
function applySuggestion(suggestionId: string) {
  const suggestion = suggestions.value.find(s => s.id === suggestionId);
  if (suggestion) {
    emit('apply-suggestion', suggestion);
    // Удаляем рекомендацию из списка
    suggestions.value = suggestions.value.filter(s => s.id !== suggestionId);
    
    if (stats.value) {
      stats.value.totalSuggestions--;
    }
  }
}

function ignoreSuggestion(suggestionId: string) {
  const suggestion = suggestions.value.find(s => s.id === suggestionId);
  if (suggestion) {
    emit('ignore-suggestion', suggestion);
    // Удаляем рекомендацию из списка
    suggestions.value = suggestions.value.filter(s => s.id !== suggestionId);
    
    if (stats.value) {
      stats.value.totalSuggestions--;
    }
  }
}

// Вспомогательные функции
function getConfidenceBadgeClass(confidence: number) {
  if (confidence >= 0.9) return 'bg-green-500/20 text-green-500';
  if (confidence >= 0.7) return 'bg-blue-500/20 text-blue-500';
  return 'bg-yellow-500/20 text-yellow-500';
}

function formatBeforeAfter(value: any): string {
  if (Array.isArray(value)) {
    return value.join(', ');
  }
  return String(value);
}

// Загрузка данных
function loadSuggestions() {
  isLoading.value = true;
  
  // Имитируем задержку загрузки данных
  setTimeout(() => {
    const response = getMockAiSuggestions(
      props.resumeId, 
      props.targetPosition, 
      props.targetCompany
    );
    
    suggestions.value = response.suggestions;
    stats.value = response.stats;
    isLoading.value = false;
  }, 1000);
}

// При монтировании компонента загружаем рекомендации
onMounted(() => {
  loadSuggestions();
});
</script>

<style scoped>
.btn {
  @apply px-6 py-3 font-semibold rounded-full transition-all duration-300 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[var(--neon-purple)];
}

.btn-primary {
  @apply bg-gradient-to-r from-[var(--neon-purple)] to-[var(--neon-blue)] text-[var(--text-light)];
}

.btn-secondary {
  @apply bg-[var(--background-section)] bg-opacity-50 text-[var(--text-light)] border border-[var(--background-pale)];
}

.confidence-badge {
  @apply font-medium;
}

.before-after-container {
  position: relative;
}

.before-after-container::after {
  content: "";
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 24px;
  height: 24px;
  background-color: var(--background-section);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 0 0 4px var(--background-section);
  z-index: 1;
}

.before-after-container::before {
  content: "→";
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: var(--neon-purple);
  font-weight: bold;
  z-index: 2;
}
</style>