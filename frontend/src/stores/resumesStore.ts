//resumesStore.ts
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
      const response = await api.get('/resumes')
      const raw = response.data
  
      console.log('[resumesStore] raw response:', raw)
  
      // ✅ Используем raw напрямую, так как это массив
      if (!Array.isArray(raw)) {
        console.warn('[resumesStore] Некорректный формат данных: ожидался массив')
        resumes.value = []
        return
      }
  
      resumes.value = raw
      console.log('[resumesStore] resumes loaded:', resumes.value)
    } catch (err) {
      console.error('[resumesStore] error loading resumes:', err)
    }
  }
  
  

  // Резюме, выбранное для редактирования
  const setResumeForEdit = (resume: Resume | null) => {
    resumeToEdit.value = resume
  }

  // Основное резюме (по ID из профиля)
  const mainResume = computed(() => {
    const profileStore = useProfileStore()
    const profile = profileStore.profile

    if (!profile || !Array.isArray(resumes.value)) return null

    return resumes.value.find(r => r.id === profile.mainResumeId) || null
  })

  // Добавление
  const addResume = async (newResume: Resume) => {
    resumes.value.push(newResume)
    await api.patch('/resumes', { resumes: resumes.value })
  }

  // Обновление
  const updateResume = async (updatedResume: Resume) => {
    const index = resumes.value.findIndex(r => r.id === updatedResume.id)
    if (index !== -1) resumes.value[index] = updatedResume
    await api.patch('/resumes', { resumes: resumes.value })
  }

  // Удаление
  const deleteResume = async (id: number) => {
    resumes.value = resumes.value.filter(r => r.id !== id)
    await api.patch('/resumes', { resumes: resumes.value })
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
