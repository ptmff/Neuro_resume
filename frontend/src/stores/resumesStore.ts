import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/api'
import type { Resume } from '@/types/types'
import { useProfileStore } from './profileStore'

export const useResumeStore = defineStore('resumes', () => {
  const resumes = ref<Resume[]>([])
  const resumeToEdit = ref<Resume | null>(null)

  const fetchResumes = async () => {
    try {
      const { data } = await api.get('resumes')
      resumes.value = Array.isArray(data) ? data : []
    } catch (err) {
      console.error('[resumesStore] Ошибка при загрузке резюме:', err)
    }
  }

  const setResumeForEdit = (resume: Resume | null) => {
    resumeToEdit.value = resume
  }

  const mainResume = computed(() => {
    const profile = useProfileStore().profile
    return profile ? resumes.value.find(r => r.id === profile.mainResumeId) || null : null
  })

  const normalizeDates = (experience: Resume['experience']) => {
    return experience.map(exp => ({
      ...exp,
      startDate: exp.startDate,
      endDate: exp.endDate
    }))
  }

  const addResume = async (resume: Resume | Omit<Resume, 'id'>) => {
    try {
      // Удаляем id, если он есть
      const { id: _, ...resumeWithoutId } = resume as Resume // принудительно считаем, что id может быть
      const formatted = {
        ...resumeWithoutId,
        experience: normalizeDates(resume.experience)
      }
  
      const { data } = await api.post('resumes', formatted)
      resumes.value.push(data)
    } catch (err) {
      console.error('[resumesStore] Ошибка при добавлении резюме:', err)
    }
  }  


  const updateResume = async (resume: Resume) => {
    try {
      const formatted = {
        ...resume,
        experience: normalizeDates(resume.experience)
      }
      await api.put(`resumes/${resume.id}`, formatted)
      const index = resumes.value.findIndex(r => r.id === resume.id)
      if (index !== -1) resumes.value[index] = formatted
    } catch (err) {
      console.error('[resumesStore] Ошибка при обновлении резюме:', err)
    }
  }

  const deleteResume = async (id: number) => {
    try {
      await api.delete(`resumes/${id}`)
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
