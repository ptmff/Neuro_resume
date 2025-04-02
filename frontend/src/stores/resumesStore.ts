import { defineStore } from 'pinia'
import { ref, computed, watch } from 'vue'
import api from '@/api'
import type { Resume } from '@/types/types'
import { useProfileStore } from './profileStore'

const LOCAL_STORAGE_KEY = 'resumeToEdit'

export const useResumeStore = defineStore('resumes', () => {
  const resumes = ref<Resume[]>([])
  const resumeToEdit = ref<Resume | null>(null)

  // Загрузка всех резюме
  const fetchResumes = async () => {
    try {
      const response = await api.get('/Resumes')
      resumes.value = response.data
      console.log('[resumesStore] resumes loaded:', resumes.value)
    } catch (err) {
      console.error('[resumesStore] error loading resumes:', err)
    }
  }

  // Добавление нового резюме
  const addResume = async (newResume: Resume) => {
    try {
      const response = await api.post('/Resumes', newResume)
      resumes.value.push(response.data)

      const profileStore = useProfileStore()
      profileStore.setMainResume(response.data.id)
      await profileStore.updateProfile({ mainResumeId: response.data.id })
    } catch (err) {
      console.error('[resumesStore] error adding resume:', err)
    }
  }

  // Обновление резюме
  const updateResume = async (updatedResume: Resume) => {
    try {
      await api.put(`/Resumes/${updatedResume.id}`, updatedResume)
      const index = resumes.value.findIndex(r => r.id === updatedResume.id)
      if (index !== -1) resumes.value[index] = updatedResume
    } catch (err) {
      console.error('[resumesStore] error updating resume:', err)
    }
  }

  // Удаление резюме
  const deleteResume = async (id: number) => {
    try {
      await api.delete(`/Resumes/${id}`)
      resumes.value = resumes.value.filter(r => r.id !== id)

      const profileStore = useProfileStore()
      if (profileStore.profile?.mainResumeId === id) {
        profileStore.setMainResume(null)
        await profileStore.updateProfile({ mainResumeId: null })
      }
    } catch (err) {
      console.error('[resumesStore] error deleting resume:', err)
    }
  }

  // Установить резюме для редактирования + сохранить в localStorage
  const setResumeForEdit = (resume: Resume | null) => {
    resumeToEdit.value = resume
    if (resume) {
      localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(resume))
    } else {
      localStorage.removeItem(LOCAL_STORAGE_KEY)
    }
  }

  // Восстановить резюме при инициализации стора
  const restoreResumeToEdit = () => {
    const saved = localStorage.getItem(LOCAL_STORAGE_KEY)
    if (saved) {
      try {
        resumeToEdit.value = JSON.parse(saved)
      } catch (err) {
        console.warn('[resumesStore] failed to parse saved resume:', err)
        localStorage.removeItem(LOCAL_STORAGE_KEY)
      }
    }
  }

  // Автоматически восстанавливаем при подключении стора
  restoreResumeToEdit()

  // Вычисляемое основное резюме
  const mainResume = computed(() => {
    const profileStore = useProfileStore()
    const profile = profileStore.profile
    if (!profile || !profile.mainResumeId) return null
    return resumes.value.find(r => r.id === profile.mainResumeId) || null
  })

  return {
    resumes,
    resumeToEdit,
    fetchResumes,
    addResume,
    updateResume,
    deleteResume,
    setResumeForEdit,
    restoreResumeToEdit,
    mainResume
  }
})
