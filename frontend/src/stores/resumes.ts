//resumes.ts
import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '@/api'
import type { Resume } from '@/types/profile'

export const useResumeStore = defineStore('resumes', () => {
  const resumes = ref<Resume[]>([])
  const resumeToEdit = ref<Resume | null>(null)

  const fetchResumes = async () => {
    try {
      const response = await api.get('/resumes')
      resumes.value = response.data.resumes
      console.log('[resumeStore] resumes:', resumes.value)
    } catch (err) {
      console.error('Ошибка при загрузке резюме:', err)
    }
  }

  const setResumeForEdit = (resume: Resume) => {
    resumeToEdit.value = resume
  }

  const addResume = async (newResume: Resume) => {
    resumes.value.push(newResume)
    await api.patch('/resumes', { resumes: resumes.value })
  }

  const updateResume = async (updatedResume: Resume) => {
    const index = resumes.value.findIndex(r => r.id === updatedResume.id)
    if (index !== -1) resumes.value[index] = updatedResume
    await api.patch('/resumes', { resumes: resumes.value })
  }

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
  }
})
