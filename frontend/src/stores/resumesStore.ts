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
      const response = await api.get('/api/resumes')
      const raw = response.data

      console.log('[resumesStore] raw response:', raw)

      if (!Array.isArray(raw)) {
        console.warn('[resumesStore] Некорректный формат данных: ожидался массив')
        resumes.value = []
        return
      }

      resumes.value = raw
    } catch (err) {
      console.error('[resumesStore] Ошибка при загрузке резюме:', err)
    }
  }

  // Резюме, выбранное для редактирования
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

  // Добавление
  const addResume = async (newResume: Omit<Resume, 'id'>) => {
    try {
      const response = await api.post('/api/resumes', newResume)
      const created = response.data
      resumes.value.push(created)
    } catch (err) {
      console.error('[resumesStore] Ошибка при добавлении резюме:', err)
    }
  }

  // Обновление
  const updateResume = async (updatedResume: Resume) => {
    try {
      await api.put(`/api/resumes/${updatedResume.id}`, updatedResume)
      const index = resumes.value.findIndex(r => r.id === updatedResume.id)
      if (index !== -1) resumes.value[index] = updatedResume
    } catch (err) {
      console.error('[resumesStore] Ошибка при обновлении резюме:', err)
    }
  }

  // Удаление
  const deleteResume = async (id: number) => {
    try {
      await api.delete(`/api/resumes/${id}`)
      resumes.value = resumes.value.filter(r => r.id !== id)
    } catch (err) {
      console.error('[resumesStore] Ошибка при удалении резюме:', err)
    }
  }

  return {
    resumes,
    resumeToEdit,
    fetchResumes,
    setResumeForEdit,
    addResume,
    updateResume,
    deleteResume,
    mainResume
  }
})
