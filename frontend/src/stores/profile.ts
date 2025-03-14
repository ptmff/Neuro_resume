import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import api from '@/api'
import type { Profile, Resume } from '@/types/profile'

export const useProfileStore = defineStore('profile', () => {
  const profile = ref<Profile | null>(null)
  const allResumes = ref<Resume[]>([])
  const isEditing = ref(false)
  const editedProfile = ref<Pick<Profile, 'name' | 'email'>>({ name: '', email: '' })
  const resumeToEdit = ref<Resume | null>(null)

  const mainResume = computed<Resume | {}>(() => {
    if (!profile.value || !allResumes.value.length) return {}
    return allResumes.value.find(r => r.id === profile.value!.mainResumeId) || {}
  })

  const fetchProfile = async () => {
    try {
      const [profileRes, resumesRes] = await Promise.all([api.get('/profile'), api.get('/resumes')])
      profile.value = profileRes.data
      allResumes.value = resumesRes.data
    } catch (err) {
      console.error('Ошибка при загрузке профиля:', err)
    }
  }

  const setMainResume = async (id: number) => {
    try {
      await api.patch('/profile', { mainResumeId: id })
      if (profile.value) profile.value.mainResumeId = id
    } catch (err) {
      console.error('Ошибка при обновлении главного резюме:', err)
    }
  }

  const startEdit = () => {
    if (!profile.value) return
    editedProfile.value = {
      name: profile.value.name,
      email: profile.value.email,
    }
    isEditing.value = true
  }

  const saveProfile = async () => {
    await updateProfile(editedProfile.value)
    isEditing.value = false
  }

  const updateProfile = async (updatedData: Partial<Profile>) => {
    try {
      await api.patch('/profile', updatedData)
      if (profile.value) profile.value = { ...profile.value, ...updatedData }
    } catch (err) {
      console.error('Ошибка при обновлении профиля:', err)
    }
  }

  const setResumeForEdit = (resume: Resume) => {
    resumeToEdit.value = resume
  }

  const deleteResume = async (id: number) => {
    if (!profile.value) return
    profile.value.resumes = profile.value.resumes.filter(rid => rid !== id)
    allResumes.value = allResumes.value.filter(r => r.id !== id)

    try {
      await api.patch('/profile', { resumes: profile.value.resumes })
      await api.patch('/resumes', { resumes: allResumes.value })
    } catch (err) {
      console.error('Ошибка при удалении резюме:', err)
    }
  }

  return {
    profile,
    allResumes,
    mainResume,
    fetchProfile,
    setMainResume,
    updateProfile,
    isEditing,
    editedProfile,
    startEdit,
    saveProfile,
    resumeToEdit,
    setResumeForEdit,
    deleteResume,
  }
})
