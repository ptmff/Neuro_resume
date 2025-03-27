import { defineStore } from 'pinia'
import { ref } from 'vue'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'

interface AnalysisResult {
  matchScore: number
  missingSkills: string[]
  overlappingExperience: boolean
  recommendations: string[]
}

interface AnalysisStep {
  type: string
  description: string
}

export const useAnalysisStore = defineStore('analysis', () => {
  const result = ref<AnalysisResult | null>(null)
  const steps = ref<AnalysisStep[]>([])

  const clear = () => {
    result.value = null
    steps.value = []
  }

  const startJobAnalysis = async (jobText: string) => {
    try {
      const profileStore = useProfileStore()
      const mainResumeId = profileStore.profile?.mainResumeId
      if (!mainResumeId) {
        throw new Error('Главное резюме не выбрано')
      }

      const resumesStore = useResumeStore()
      const resume = resumesStore.resumes.find(r => r.id === mainResumeId)
      if (!resume) {
        throw new Error('Не удалось найти резюме с таким ID')
      }

      const res = await fetch('/analyse', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          JobText: jobText,
          resume,
        }),
      })

      const data = await res.json()
      result.value = data.result
      steps.value = data.steps
    } catch (err) {
      console.error('[analysisStore] Анализ вакансии не удался:', err)
      throw err
    }
  }

  return {
    result,
    steps,
    clear,
    startJobAnalysis,
  }
})
