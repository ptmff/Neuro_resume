import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/api'
import type { Resume } from '@/types/types'
import { useProfileStore } from './profileStore'

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

  // Добавление
  const addResume = async (newResume: Resume) => {
    try {
      const response = await api.post('/Resumes', newResume)
      resumes.value.push(response.data) // сервер вернёт новое резюме с ID
    } catch (err) {
      console.error('[resumesStore] error adding resume:', err)
    }
  }

  // Обновление
  const updateResume = async (updatedResume: Resume) => {
    try {
      await api.put(`/Resumes/${updatedResume.id}`, updatedResume)
      const index = resumes.value.findIndex(r => r.id === updatedResume.id)
      if (index !== -1) resumes.value[index] = updatedResume
    } catch (err) {
      console.error('[resumesStore] error updating resume:', err)
    }
  }

  // Удаление
  const deleteResume = async (id: number) => {
    try {
      await api.delete(`/Resumes/${id}`)
      resumes.value = resumes.value.filter(r => r.id !== id)
    } catch (err) {
      console.error('[resumesStore] error deleting resume:', err)
    }
  }

  // Выбор резюме для редактирования
  const setResumeForEdit = (resume: Resume | null) => {
    resumeToEdit.value = resume
  }

  // Основное резюме
  const mainResume = computed(() => {
    const profileStore = useProfileStore()
    const profile = profileStore.profile

    if (!profile || !Array.isArray(resumes.value)) return null
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
    mainResume
  }
})
