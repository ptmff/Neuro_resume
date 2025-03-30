import { defineStore } from 'pinia'
import { ref, nextTick } from 'vue'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'
import {
  playSound,
  showParticles,
  animateSection,
  highlightMatchedSkills,
  animateTimeline
} from '@/composables/useAnalysisAnimations'

import { fetchMockAnalysis } from '@/mocks/mockAnalysis' // 🧪 подключаем мок

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

export const useAnalysisStore = defineStore('analysisStore', () => {
  const analysisMode = ref<'fast' | 'deep'>('deep')
  const result = ref<AnalysisResult | null>(null)
  const currentPhaseIndex = ref(0)
  const isBackendDone = ref(false)

  const phases = ref<AnalysisPhase[]>([
    {
      id: 'connect',
      title: 'Подключение к вакансии',
      description: 'Устанавливаем связь...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1000,
      requiredInFastMode: true,
      onStart: () => playSound('connect'),
      onComplete: () => showParticles('connect')
    },
    {
      id: 'scanSkills',
      title: 'Анализ навыков',
      description: 'Сравниваем навыки...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1500,
      requiredInFastMode: true,
      onStart: () => animateSection('skills'),
      onComplete: () => highlightMatchedSkills()
    },
    {
      id: 'matchExperience',
      title: 'Опыт',
      description: 'Оцениваем опыт...',
      status: 'pending',
      logicalProgress: 0,
      visualProgress: 0,
      duration: 1200,
      requiredInFastMode: false,
      onStart: () => animateTimeline()
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
  ])

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

      // 🧪 МОК вместо бэка:
      const data = await fetchMockAnalysis(jobText)
      result.value = data.result
      isBackendDone.value = true

      // 🔁 Реальный вызов позже:
      /*
      const res = await fetch('/api/analyse', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ jobText, resume })
      })

      const data = await res.json()
      result.value = data.result
      isBackendDone.value = true
      */

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

  const startJobAnalysis = async (jobText: string) => {
    clear()
    isBackendDone.value = false
    fetchAnalysisResult(jobText) // 🧪 идёт параллельно
    await nextTick()
    await startAnalysisPhases()
  }

  const clear = () => {
    result.value = null
    isBackendDone.value = false
    currentPhaseIndex.value = 0
    phases.value.forEach(p => {
      p.status = 'pending'
      p.logicalProgress = 0
      p.visualProgress = 0
    })
  }

  const setMode = (mode: 'fast' | 'deep') => {
    analysisMode.value = mode
  }

  return {
    analysisMode,
    setMode,
    phases,
    currentPhaseIndex,
    result,
    startJobAnalysis,
    clear,
    isBackendDone
  }
})
