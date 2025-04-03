import { defineStore } from 'pinia'
import { ref, computed, nextTick } from 'vue'
import api from '@/api'
import type { Resume } from '@/types/types'
import { useProfileStore } from './profileStore'

const LOCAL_STORAGE_KEY = 'resumeToEdit'

// Локальные типы, если нет в @/types/types
export interface AiSuggestion {
  id: string;
  type: 'skills' | 'experience' | 'education' | 'description';
  title: string;
  description: string;
  confidence: number;
  before: any;
  after: any;
  reasoning: string;
  targetExperienceId?: string | null;
}

interface AiSuggestionsResponse {
  success: boolean;
  resumeId: number;
  suggestions: AiSuggestion[];
  stats: {
    totalSuggestions: number;
    estimatedImprovementScore: number;
    targetPositionMatch: {
      before: number;
      after: number;
    };
  };
}

export const useResumeStore = defineStore('resumes', () => {
  const resumes = ref<Resume[]>([])
  const resumeToEdit = ref<Resume | null>(null)
  const aiSuggestions = ref<AiSuggestion[]>([])
  const aiStats = ref<AiSuggestionsResponse['stats'] | null>(null)

  const fetchResumes = async () => {
    try {
      const response = await api.get('/Resumes')
      resumes.value = response.data
      console.log('[resumesStore] resumes loaded:', resumes.value)
    } catch (err) {
      console.error('[resumesStore] error loading resumes:', err)
    }
  }

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

  const updateResume = async (updatedResume: Resume) => {
    try {
      await api.put(`/Resumes/${updatedResume.id}`, updatedResume)
      const index = resumes.value.findIndex((r: Resume) => r.id === updatedResume.id)
      if (index !== -1) resumes.value[index] = updatedResume
    } catch (err) {
      console.error('[resumesStore] error updating resume:', err)
    }
  }

  const deleteResume = async (id: number) => {
    try {
      await api.delete(`/Resumes/${id}`)
      resumes.value = resumes.value.filter((r: Resume) => r.id !== id)

      const profileStore = useProfileStore()
      if (profileStore.profile?.mainResumeId === id) {
        profileStore.setMainResume(null)
        await profileStore.updateProfile({ mainResumeId: null })
      }
    } catch (err) {
      console.error('[resumesStore] error deleting resume:', err)
    }
  }

  const setResumeForEdit = (resume: Resume | null) => {
    resumeToEdit.value = resume
    if (resume) {
      localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(resume))
    } else {
      localStorage.removeItem(LOCAL_STORAGE_KEY)
    }
  }

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

  const isAnalyzing = ref(false)

  const analyzeResume = async (resume: Resume) => {
    isAnalyzing.value = true
    try {
      const { id, ...resumeWithoutId } = resume
      const response = await api.post<AiSuggestion[]>('/ResumeAnalysis/analyze', resumeWithoutId)
      aiSuggestions.value = Array.isArray(response.data) ? response.data : []
      aiStats.value = null // если stats не приходит
    } catch (err) {
      console.error('[resumesStore] error during AI resume analysis:', err)
    } finally {
      isAnalyzing.value = false
    }
  }
  
  
  

  restoreResumeToEdit()

  const mainResume = computed(() => {
    const profileStore = useProfileStore()
    const profile = profileStore.profile
    if (!profile || !profile.mainResumeId) return null
    return resumes.value.find((r: Resume) => r.id === profile.mainResumeId) || null
  })

  return {
    resumes,
    resumeToEdit,
    aiSuggestions,
    aiStats,
    isAnalyzing,
    fetchResumes,
    addResume,
    updateResume,
    deleteResume,
    setResumeForEdit,
    restoreResumeToEdit,
    mainResume,
    analyzeResume
  }
})