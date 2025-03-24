// appStore.ts
import { defineStore } from 'pinia'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'
import { ref } from 'vue'

export const useAppStore = defineStore('app', () => {
  const isAppLoading = ref(true)
  const isAppReady = ref(false)
  const error = ref<string | null>(null)
  // const analysisResult = ref<any>(null)

  const initialize = async () => {
    isAppLoading.value = true
    error.value = null

    try {
      const profileStore = useProfileStore()
      const resumesStore = useResumeStore()

      await profileStore.fetchProfile()
      await resumesStore.fetchResumes()

      isAppReady.value = true
    } catch (err: any) {
      error.value = err.message || 'Ошибка при инициализации приложения'
      console.error('[appStore] Init error:', err)
    } finally {
      isAppLoading.value = false
    }
  }

  const reset = () => {
    isAppLoading.value = false
    isAppReady.value = false
    error.value = null
  }

  const logout = () => {
    const profileStore = useProfileStore()
    const resumesStore = useResumeStore()

    profileStore.$reset()
    resumesStore.$reset()
    reset()
  }

  // const startJobAnalysis = async (JobText: string) => {
  //   try {
  //     const profileStore = useProfileStore()
  //     const mainResumeId = profileStore.profile?.mainResumeId
  //     if (!mainResumeId) {
  //       throw new Error('Главное резюме не выбрано')
  //     }

  //     const resumesStore = useResumeStore()
  //     const resume = resumesStore.resumes.find(r => r.id === mainResumeId)
  //     if (!resume) {
  //       throw new Error('Не удалось найти резюме с таким ID')
  //     }

  //     const res = await fetch('/analyse', {
  //       method: 'POST',
  //       headers: {
  //         'Content-Type': 'application/json',
  //       },
  //       body: JSON.stringify({
  //         JobText,
  //         resume
  //       }),
  //     })

  //     const result = await res.json()
  //     analysisResult.value = result
  //   } catch (err) {
  //     console.error('[appStore] Анализ вакансии не удался:', err)
  //     throw err
  //   }
  // }

  const analysisResult = ref<null | {
    summary: {
      match: number,
      skills: number,
      experience: number,
      keywords: number
    },
    insights: { text: string }[],
    skills: {
      matched: string[],
      missing: string[],
      similar: string[]
    },
    timeline: {
      matched: string[],
      gap: string[]
    }
  }>(null)
  

  const startJobAnalysis = async (jobText: string) => {
    try {
      const profileStore = useProfileStore()
      const resumesStore = useResumeStore()
      const mainResumeId = profileStore.profile?.mainResumeId
      const resume = resumesStore.resumes.find(r => r.id === mainResumeId)
  
      // 💤 Имитируем задержку
      await new Promise(res => setTimeout(res, 1000))
  
      // 💡 Мок-ответ
      const mockResult = {
        summary: {
          match: 82,
          skills: 75,
          experience: 66,
          keywords: 90
        },
        insights: [
          { text: 'Добавьте Vue.js — это ключевой навык' },
          { text: 'Уточните опыт командной работы' }
        ],
        skills: {
          matched: ['Vue', 'REST'],
          missing: ['TypeScript'],
          similar: ['JS → TS']
        },
        timeline: {
          matched: ['2022–2023'],
          gap: ['2021']
        }
      }
  
      // 🔄 Сохрани в appStore (например, в `analysisResult`)
      analysisResult.value = mockResult
  
    } catch (err) {
      console.error('[appStore] Анализ вакансии не удался:', err)
      throw err
    }
  }
  

  return {
    isAppLoading,
    isAppReady,
    error,
    initialize,
    reset,
    logout,
    startJobAnalysis,
    analysisResult 
  }
})
