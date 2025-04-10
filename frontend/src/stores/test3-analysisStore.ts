import { defineStore } from 'pinia'
import { ref, watch, nextTick } from 'vue'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'
import {
  playSound,
  showParticles,
  animateSection,
  highlightMatchedSkills,
  animateTimeline
} from '@/composables/useAnalysisAnimations'
import { fetchMockAnalysis } from '@/mocks/mockAnalysis'
import api from '@/api'

interface AnalysisPhase {
  id: string
  title: string
  description: string
  status: 'pending' | 'active' | 'done' | 'skipped'
  logicalProgress: number
  visualProgress: number
  duration?: number
  requiredInFastMode?: boolean
  onStart?: () => void
  onComplete?: () => void
}

interface AnalysisResult {
  matchScore: number
  missingSkills: string[]
  overlappingExperience: boolean
  recommendations: string[]
}

const STORAGE_KEY = 'neuro-analysis-state'

export const useAnalysisStore = defineStore('analysisStore', () => {
  const analysisMode = ref<'fast' | 'deep'>('deep')
  const result = ref<AnalysisResult | null>(null)
  const currentPhaseIndex = ref(0)
  const isBackendDone = ref(false)
  const jobUrl = ref<string>('')

  const getDefaultPhases = (): AnalysisPhase[] => [
    {
      id: 'connect',
      title: 'Подключение к вакансии',
      description: 'Устанавливаем связь...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1000,
      requiredInFastMode: true
    },
    {
      id: 'scanSkills',
      title: 'Анализ навыков',
      description: 'Сравниваем навыки...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1500,
      requiredInFastMode: true
    },
    {
      id: 'matchExperience',
      title: 'Опыт',
      description: 'Оцениваем опыт...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1200,
      requiredInFastMode: false
    },
    {
      id: 'aiModel',
      title: 'AI-моделирование',
      description: 'Строим модель...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1800
    },
    {
      id: 'finalize',
      title: 'Завершение',
      description: 'Финализируем результат...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1000
    }
  ]

  const injectPhaseCallbacks = (phases: AnalysisPhase[]) => {
    for (const phase of phases) {
      if (phase.id === 'connect') {
        phase.onStart = () => playSound('connect')
        phase.onComplete = () => showParticles('connect')
      } else if (phase.id === 'scanSkills') {
        phase.onStart = () => animateSection('skills')
        phase.onComplete = () => highlightMatchedSkills()
      } else if (phase.id === 'matchExperience') {
        phase.onStart = () => animateTimeline()
      }
    }
  }

  const phases = ref<AnalysisPhase[]>(getDefaultPhases())
  injectPhaseCallbacks(phases.value)

  const simulatePhase = (phase: AnalysisPhase): Promise<void> => {
    return new Promise((resolve) => {
      const duration = phase.duration || 1000
      let logical = 0
      let visual = 0
      const step = 16

      const interval = setInterval(() => {
        logical += step
        visual += step * 1.1
        phase.logicalProgress = Math.min((logical / duration) * 100, 100)
        phase.visualProgress = Math.min((visual / duration) * 100, 100)

        if (logical >= duration) {
          clearInterval(interval)
          resolve()
        }
      }, step)
    })
  }

  const fetchAnalysisResult = async (jobText: string) => {
    try {
      const profileStore = useProfileStore()
      const resumesStore = useResumeStore()
  
      const mainResumeId = profileStore.profile?.mainResumeId
      if (!mainResumeId) throw new Error('Главное резюме не выбрано')
  
      const resume = resumesStore.resumes.find(r => r.id === mainResumeId)
      if (!resume) throw new Error('Не найдено резюме')
  
      const { data } = await api.post('/ResumeAnalysis/analyzeVacancy', {
        resume,
        vacancyUrl: jobText
      })
  
      result.value = data
      isBackendDone.value = true
    } catch (err) {
      console.error('[analysisStore] Ошибка анализа:', err)
      throw err
    }
  }

  const startAnalysisPhases = async () => {
    for (let i = 0; i < phases.value.length; i++) {
      const phase = phases.value[i]
      const shouldSkip = analysisMode.value === 'fast' && !phase.requiredInFastMode

      if (shouldSkip) {
        phase.status = 'skipped'
        currentPhaseIndex.value = i + 1
        continue
      }

      phase.status = 'active'
      currentPhaseIndex.value = i
      phase.onStart?.()

      await nextTick()
      await simulatePhase(phase)

      phase.status = 'done'
      phase.onComplete?.()
      currentPhaseIndex.value = i + 1

      if (i === phases.value.length - 1 && !isBackendDone.value) {
        while (!isBackendDone.value) {
          await new Promise(resolve => setTimeout(resolve, 300))
        }
      }
    }
  }

  const startJobAnalysis = async (text: string) => {
    clear()
    isBackendDone.value = false
    jobUrl.value = text
    fetchAnalysisResult(text)
    await nextTick()
    await startAnalysisPhases()
  }

  const clear = () => {
    result.value = null
    isBackendDone.value = false
    currentPhaseIndex.value = 0
    jobUrl.value = ''
    phases.value = getDefaultPhases()
    injectPhaseCallbacks(phases.value)
  }

  const setMode = (mode: 'fast' | 'deep') => {
    analysisMode.value = mode
  }

  // 🧠 Хранилище в localStorage
  const saveToLocalStorage = () => {
    const data = {
      analysisMode: analysisMode.value,
      currentPhaseIndex: currentPhaseIndex.value,
      result: result.value,
      isBackendDone: isBackendDone.value,
      phases: phases.value.map(({ onStart, onComplete, ...p }) => p),
      jobUrl: jobUrl.value
    }
    localStorage.setItem(STORAGE_KEY, JSON.stringify(data))
  }

  const loadFromLocalStorage = () => {
    try {
      const data = JSON.parse(localStorage.getItem(STORAGE_KEY) || '{}')
      if (!data || !data.phases) return

      analysisMode.value = data.analysisMode || 'deep'
      currentPhaseIndex.value = data.currentPhaseIndex || 0
      result.value = data.result || null
      isBackendDone.value = data.isBackendDone || false
      jobUrl.value = data.jobUrl || ''
      phases.value = data.phases
      injectPhaseCallbacks(phases.value)
    } catch (err) {
      console.error('[analysisStore] ошибка загрузки состояния:', err)
    }
  }

  // 🧷 Автосохранение
  watch(
    [phases, result, isBackendDone, currentPhaseIndex, analysisMode, jobUrl],
    saveToLocalStorage,
    { deep: true }
  )

  loadFromLocalStorage()

  return {
    analysisMode,
    setMode,
    phases,
    currentPhaseIndex,
    result,
    startJobAnalysis,
    clear,
    isBackendDone,
    jobUrl
  }
})
