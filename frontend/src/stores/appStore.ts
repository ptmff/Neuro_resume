// appStore.ts
import { defineStore } from 'pinia'
import { useProfileStore } from './profileStore'
import { useResumeStore } from './resumesStore'
import { useAuthStore } from './authStore'
import { ref } from 'vue'

export const useAppStore = defineStore('app', () => {
  const isAppLoading = ref(true)
  const isAppReady = ref(false)
  const error = ref<string | null>(null)

  const initialize = async () => {
    isAppLoading.value = true
    error.value = null

    try {
      const authStore = useAuthStore()
      const profileStore = useProfileStore()
      const resumesStore = useResumeStore()

      // Только если уже есть токен
      if (authStore.token) {
        await profileStore.fetchProfile()
        await resumesStore.fetchResumes()
        isAppReady.value = true
      } else {
        // Не считаем ошибкой — просто пользователь не залогинен
        isAppReady.value = false
      }
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
    const authStore = useAuthStore()

    authStore.logout()
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
    logout,
  }
})
