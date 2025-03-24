// appStore.ts
import { defineStore } from 'pinia'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'
import { ref } from 'vue'

export const useAppStore = defineStore('app', () => {
  const isAppLoading = ref(true)
  const isAppReady = ref(false)
  const error = ref<string | null>(null)
  const analysisResult = ref<any>(null)

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

  const startJobAnalysis = async (JobText: string) => {
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
          JobText,
          resume
        }),
      })

      const result = await res.json()
      analysisResult.value = result
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
    startJobAnalysis
  }
})
