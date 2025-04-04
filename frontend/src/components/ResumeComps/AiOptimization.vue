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
      <div class="flex flex-col items-center">
        <div class="animate-spin rounded-full h-16 w-16 border-t-2 border-b-2 border-[var(--neon-purple)] mb-4"></div>
        <p class="text-[var(--text-secondary)]">Анализируем ваше резюме...</p>
        <p class="text-xs text-[var(--text-mainless)] mt-2">Это может занять некоторое время</p>
      </div>
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
            <div class="text-3xl font-bold text-[var(--text-light)]">{{ suggestionsCount }}</div>
            <p class="text-xs text-[var(--text-mainless)]">Рекомендации</p>
          </div>
        </div>
      </div>

      <!-- Применённая рекомендация -->
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

      <!-- Рекомендации -->
      <div v-if="suggestionsCount > 0" class="space-y-4">
        <div
          v-for="suggestion in suggestions"
          :key="suggestion.id"
          class="suggestion-item p-4 rounded-xl bg-[var(--background-section)] bg-opacity-50 border border-[var(--background-pale)]"
        >
          <div class="flex justify-between items-start mb-2">
            <h4 class="text-xl font-semibold text-[var(--text-light)]">{{ suggestion.title }}</h4>
            <div class="confidence-badge px-2 py-1 rounded-full text-xs" :class="getConfidenceBadgeClass(suggestion.confidence)">
              {{ Math.round(suggestion.confidence * 100) }}% уверенность
            </div>
          </div>
          <p class="text-[var(--text-secondary)] mb-4">{{ suggestion.description }}</p>

          <div class="before-after-container mb-4 p-3 rounded-lg bg-[var(--background-section)] bg-opacity-70">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <p class="text-xs text-[var(--text-mainless)] mb-1">Сейчас:</p>
                <div class="p-2 rounded bg-[var(--background-pale)] bg-opacity-30 text-sm text-[var(--text-secondary)]">
                  <pre class="whitespace-pre-wrap break-words">{{ formatBeforeAfter(suggestion.before, suggestion.type) }}</pre>
                </div>
              </div>
              <div>
                <p class="text-xs text-[var(--text-mainless)] mb-1">Предлагаемое:</p>
                <div class="p-2 rounded bg-[var(--neon-purple)]/5 border border-[var(--neon-purple)]/10 text-sm text-[var(--text-light)]">
                  <pre class="whitespace-pre-wrap break-words">{{ formatBeforeAfter(suggestion.after, suggestion.type) }}</pre>
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
      <div v-if="suggestionsCount === 0 && !isLoading" class="no-suggestions p-6 text-center">
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
import { ref, onMounted, computed, watch } from 'vue'
import { useResumeStore } from '@/stores/resumesStore'
import type { AiSuggestion } from '@/types/types'

// Типы
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

// Props & Emits
const props = defineProps<{ 
  resumeData: ResumeData; 
  aiSuggestions?: AiSuggestion[];
  modelValue?: ResumeData;
}>()

const emit = defineEmits(['apply-suggestion', 'next-step', 'prev-step', 'update:modelValue'])

// Store
const resumeStore = useResumeStore()

// Состояния
const isLoading = computed(() => resumeStore.isAnalyzing)
const suggestions = computed(() => resumeStore.aiSuggestions)
const stats = computed(() => resumeStore.aiStats)
const lastAppliedSuggestion = ref<AiSuggestion | null>(null)
const suggestionsCount = computed(() => suggestions.value?.length || 0)
const isApplyingChanges = ref(false)

// Применить улучшение
const applySuggestion = async (suggestion: AiSuggestion) => {
  isApplyingChanges.value = true
  
  try {
    // Create a deep copy of the current resumeData
    const updated = JSON.parse(JSON.stringify(props.resumeData))
    
    // Apply changes based on suggestion type
    switch (suggestion.type) {
      case 'title':
        updated.title = suggestion.after as string
        break
        
      case 'skills':
        if (Array.isArray(suggestion.after)) {
          updated.skills = [...suggestion.after]
        }
        break
        
      case 'experience':
        if (Array.isArray(suggestion.after)) {
          // Handle case where entire experience array is replaced
          updated.experience = [...suggestion.after]
        } else if (suggestion.targetExperienceId && updated.experience) {
          // Handle case where a specific experience entry is updated
          const index = updated.experience.findIndex((e: { id: string | null | undefined; }) => e.id === suggestion.targetExperienceId)
          if (index !== -1) {
            if (typeof suggestion.after === 'object') {
              updated.experience[index] = {
                ...updated.experience[index],
                ...suggestion.after
              }
            } else {
              updated.experience[index].description = suggestion.after as string
            }
          }
        } else if (updated.experience && updated.experience.length > 0) {
          // Fallback to updating the first experience entry
          if (typeof suggestion.after === 'object') {
            updated.experience[0] = {
              ...updated.experience[0],
              ...suggestion.after
            }
          } else {
            updated.experience[0].description = suggestion.after as string
          }
        }
        break
        
      case 'description':
        updated.description = suggestion.after as string
        break
        
      case 'education':
        if (Array.isArray(suggestion.after)) {
          updated.education = [...suggestion.after]
        }
        break
    }
    
    // Update the parent component with the new data
    emit('update:modelValue', updated)
    
    // Also update the store if needed
    if (resumeStore.resumeToEdit && props.resumeData.id === resumeStore.resumeToEdit.id) {
      // Apply the suggestion directly to the store
      resumeStore.applySuggestion(suggestion)
    }
    
    // Show the applied suggestion notification
    lastAppliedSuggestion.value = suggestion
    
    // Remove the suggestion from the list
    resumeStore.aiSuggestions = resumeStore.aiSuggestions.filter(s => s.id !== suggestion.id)
    
    // Update stats if available
    if (resumeStore.aiStats) {
      resumeStore.aiStats.totalSuggestions--
    }
    
    // Emit the event for parent components
    emit('apply-suggestion', suggestion)
    
    // Auto-hide the notification after 5 seconds
    setTimeout(() => {
      if (lastAppliedSuggestion.value?.id === suggestion.id) {
        lastAppliedSuggestion.value = null
      }
    }, 5000)
  } catch (error) {
    console.error('[AiOptimization] Error applying suggestion:', error)
  } finally {
    isApplyingChanges.value = false
  }
}

// Игнорировать рекомендацию
const ignoreSuggestion = (id: string) => {
  resumeStore.ignoreSuggestion(id)
}

// Отображение значка уверенности
const getConfidenceBadgeClass = (n: number) =>
  n >= 0.9 ? 'bg-green-500/20 text-green-500' :
  n >= 0.7 ? 'bg-blue-500/20 text-blue-500' :
             'bg-yellow-500/20 text-yellow-500'

// Форматирование для отображения before/after значений
const formatBeforeAfter = (v: unknown, type?: string): string => {
  if (v === null || v === undefined) {
    return 'Нет данных'
  }
  
  // Special handling for experience type
  if (type === 'experience' && Array.isArray(v)) {
    // For experience, only show the description field
    if (v.length > 0 && typeof v[0] === 'object' && 'description' in v[0]) {
      return v[0].description as string
    }
  }
  
  if (Array.isArray(v)) {
    if (v.length === 0) {
      return 'Пустой список'
    }
    
    // For skills and other simple arrays
    if (typeof v[0] !== 'object') {
      return v.join(', ')
    }
    
    // For complex objects, still use JSON but with better formatting
    return JSON.stringify(v, null, 2)
  }
  
  if (typeof v === 'object') {
    return JSON.stringify(v, null, 2)
  }
  
  return String(v)
}

// Загрузка при монтировании
const loadSuggestions = async () => {
  if (props.aiSuggestions?.length) {
    resumeStore.aiSuggestions = props.aiSuggestions
    return
  }

  try {
    // Ensure we have all required fields for analysis
    await resumeStore.analyzeResume({
      title: props.resumeData.title || '',
      date: props.resumeData.date || new Date().toISOString(),
      job: props.resumeData.job || '',
      skills: props.resumeData.skills || [],
      city: props.resumeData.city || '',
      experience: props.resumeData.experience || [],
      education: props.resumeData.education || [],
      template: props.resumeData.template || '',
      description: props.resumeData.description || '',
      id: props.resumeData.id || 0
    })
  } catch (e) {
    console.error('[ResumeAiOptimization] Ошибка анализа резюме:', e)
  }
}

// Watch for changes to props.resumeData and update if needed
watch(() => props.resumeData, (newValue) => {
  if (newValue && !isApplyingChanges.value) {
    // Only reload suggestions if this is a completely different resume
    if (resumeStore.resumeToEdit?.id !== newValue.id) {
      loadSuggestions()
    }
  }
}, { deep: true })

onMounted(loadSuggestions)
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

