// appStore.ts
import { defineStore } from 'pinia'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'
import { ref } from 'vue'

export const useAppStore = defineStore('app', () => {
  const isAppLoading = ref(true)
  const isAppReady = ref(false)
  const error = ref<string | null>(null)

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

  return {
    isAppLoading,
    isAppReady,
    error,
    initialize,
    reset,
    logout
  }
})
