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

      <!-- Последнее примененное изменение -->
      <div v-if="lastAppliedSuggestion" class="applied-suggestion-container mb-6 p-4 rounded-xl bg-[var(--background-section)] bg-opacity-70 border border-green-300">
        <div class="flex items-center justify-between mb-3">
          <div class="flex items-center">
            <div class="bg-green-500 p-2 rounded-full mr-3">
              <i class="fas fa-check text-white"></i>
            </div>
            <h4 class="text-lg font-semibold text-[var(--text-light)]">Изменение применено</h4>
          </div>
          <button @click="lastAppliedSuggestion = null" class="text-[var(--text-mainless)] hover:text-[var(--text-light)]">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <p class="text-[var(--text-secondary)] mb-2">{{ lastAppliedSuggestion.title }}</p>
        <div class="text-sm text-green-500">
          <i class="fas fa-eye mr-1"></i> Изменения отображаются в превью справа
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
            <button @click="applySuggestion(suggestion)" class="btn btn-primary">
              <i class="fas fa-check mr-2"></i> Применить
            </button>
            <button @click="ignoreSuggestion(suggestion.id)" class="btn btn-secondary">
              <i class="fas fa-times mr-2"></i> Игнорировать
            </button>
          </div>
        </div>
      </div>
      
      <!-- Нет рекомендаций -->
      <div v-if="suggestions.length === 0 && !isLoading" class="no-suggestions p-6 text-center">
        <div class="bg-green-500/10 p-6 rounded-xl border border-green-500/20 mb-4">
          <i class="fas fa-check-circle text-green-500 text-4xl mb-3"></i>
          <h4 class="text-xl font-semibold text-[var(--text-light)] mb-2">Ваше резюме оптимизировано!</h4>
          <p class="text-[var(--text-secondary)]">Все рекомендации были применены. Ваше резюме готово к отправке.</p>
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
import { useResumeStore } from '@/stores/resumesStore';

// Типы
import type { AiSuggestion } from '@/stores/resumesStore';

interface Experience {
  id?: string;
  company: string;
  position: string;
  startDate: string;
  endDate: string;
  description: string;
}

interface ResumeData {
  id?: number;
  title?: string;
  name?: string;
  email?: string;
  phone?: string;
  city?: string;
  job?: string;
  description?: string;
  experience?: Experience[];
  skills?: string[];
  template?: string;
  date?: string;
  sectionsOrder?: string[];
  education?: {
    institution: string;
    degree: string;
    field: string;
    startYear: number;
    endYear: number;
  }[];
  [key: string]: any;
}

interface Props {
  resumeData: ResumeData;
  aiSuggestions?: AiSuggestion[];
}

interface Emits {
  (e: 'apply-suggestion', suggestion: AiSuggestion): void;
  (e: 'next-step'): void;
  (e: 'prev-step'): void;
  (e: 'update:modelValue', value: ResumeData): void;
}

const props = defineProps<Props>();
const emit = defineEmits<Emits>();

const resumeStore = useResumeStore();

const isLoading = ref(true);
const suggestions = computed(() => resumeStore.aiSuggestions);
const stats = computed(() => resumeStore.aiStats);
const lastAppliedSuggestion = ref<AiSuggestion | null>(null);

const applySuggestion = (suggestion: AiSuggestion): void => {
  lastAppliedSuggestion.value = suggestion;
  const updatedResumeData: ResumeData = { ...props.resumeData };

  switch (suggestion.type) {
    case 'skills':
      if (Array.isArray(suggestion.after)) {
        updatedResumeData.skills = [...suggestion.after];
      }
      break;
    case 'experience':
      updatedResumeData.experience = [...(updatedResumeData.experience || [])];
      if (suggestion.targetExperienceId) {
        const index = updatedResumeData.experience.findIndex(
          (exp: Experience) => exp.id === suggestion.targetExperienceId
        );
        if (index !== -1) {
          updatedResumeData.experience[index] = {
            ...updatedResumeData.experience[index],
            description: suggestion.after as string
          };
        }
      }
      if (
        updatedResumeData.experience.length > 0 &&
        (!suggestion.targetExperienceId ||
         !updatedResumeData.experience.some((exp: Experience) => exp.id === suggestion.targetExperienceId))
      ) {
        updatedResumeData.experience[0] = {
          ...updatedResumeData.experience[0],
          description: suggestion.after as string
        };
      }
      break;
    case 'description':
      updatedResumeData.description = suggestion.after as string;
      break;
  }

  emit('update:modelValue', updatedResumeData);
  emit('apply-suggestion', suggestion);

  resumeStore.aiSuggestions = resumeStore.aiSuggestions.filter((s: AiSuggestion) => s.id !== suggestion.id);

  if (resumeStore.aiStats) resumeStore.aiStats.totalSuggestions--;

  setTimeout(() => {
    lastAppliedSuggestion.value = null;
  }, 5000);
};

const ignoreSuggestion = (suggestionId: string): void => {
  resumeStore.aiSuggestions = resumeStore.aiSuggestions.filter((s: AiSuggestion) => s.id !== suggestionId);
  if (resumeStore.aiStats) resumeStore.aiStats.totalSuggestions--;
};

const getConfidenceBadgeClass = (confidence: number): string => {
  if (confidence >= 0.9) return 'bg-green-500/20 text-green-500';
  if (confidence >= 0.7) return 'bg-blue-500/20 text-blue-500';
  return 'bg-yellow-500/20 text-yellow-500';
};

const formatBeforeAfter = (value: unknown): string => {
  if (Array.isArray(value)) return value.join(', ');
  return String(value);
};

const loadSuggestions = async (): Promise<void> => {
  isLoading.value = true;

  if (props.aiSuggestions && props.aiSuggestions.length > 0) {
    resumeStore.aiSuggestions = props.aiSuggestions;
    isLoading.value = false;
    return;
  }

  try {
    await resumeStore.analyzeResume({
      title: props.resumeData.title || '',
      date: new Date().toISOString(),
      job: props.resumeData.job || '',
      skills: props.resumeData.skills || [],
      city: props.resumeData.city || '',
      experience: props.resumeData.experience || [],
      education: props.resumeData.education || [],
      template: props.resumeData.template || '',
      description: props.resumeData.description || '',
      id: 0
    });
  } catch (e) {
    console.error('[ResumeAiOptimization] Ошибка анализа резюме:', e);
  } finally {
    isLoading.value = false;
  }
};

onMounted(loadSuggestions);
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

.applied-suggestion-container {
  animation: slideDown 0.5s ease-out;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>