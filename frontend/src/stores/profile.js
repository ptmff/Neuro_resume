import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import api from '@/api'

export const useProfileStore = defineStore('profile', () => {
  const profile = ref(null)
  const allResumes = ref([])
  const isEditing = ref(false)
  const editedProfile = ref({ name: '', email: '' })
  const resumeToEdit = ref(null)


  const mainResume = computed(() => {
    if (!profile.value || !allResumes.value.length) return {}
    return allResumes.value.find(r => r.id === profile.value.mainResumeId) || {}
  })

  const fetchProfile = async () => {
    try {
      const [profileRes, resumesRes] = await Promise.all([
        api.get('/profile'),
        api.get('/resumes')
      ])
      profile.value = profileRes.data
      allResumes.value = resumesRes.data
    } catch (err) {
      console.error('Ошибка при загрузке профиля:', err)
    }
  }

  const setMainResume = async (id) => {
    try {
      await api.patch('/profile', { mainResumeId: id })
      profile.value.mainResumeId = id
    } catch (err) {
      console.error('Ошибка при обновлении главного резюме:', err)
    }
  }

  const startEdit = () => {
    editedProfile.value = {
      name: profile.value.name,
      email: profile.value.email
    }
    isEditing.value = true
  }
  
  const saveProfile = async () => {
    await updateProfile(editedProfile.value)
    isEditing.value = false
  }  

  const updateProfile = async (updatedData) => {
    try {
      await api.patch('/profile', updatedData)
      profile.value = { ...profile.value, ...updatedData }
    } catch (err) {
      console.error('Ошибка при обновлении профиля:', err)
    }
  }

  const setResumeForEdit = (resume) => {
    resumeToEdit.value = resume
  }
  
  const deleteResume = async (id) => {
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
    deleteResume
  }  
})
