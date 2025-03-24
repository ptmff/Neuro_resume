import api from "@/api"
import { Profile } from "@/types/types"
import { defineStore } from "pinia"
import { ref } from "vue"

export const useProfileStore = defineStore('profile', () => {
  const profile = ref<Profile | null>(null)
  const isEditing = ref(false)
  const editedProfile = ref<Pick<Profile, 'name' | 'email'>>({ name: '', email: '' })

  const fetchProfile = async () => {
    try {
      const profileRes = await api.get('/profile')
      const raw = profileRes.data

      // Проверка на корректность поля resumes
      if (!Array.isArray(raw.resumes)) {
        console.warn('[profileStore] Некорректный формат данных: поле "resumes" не массив ID')
        raw.resumes = []
      }

      profile.value = raw
      console.log('[profileStore] profile:', profile.value)
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
      email: profile.value.email
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

  const addResumeId = async (id: number) => {
    if (!profile.value) return
  
    const updatedResumes = [...profile.value.resumes, id]
  
    try {
      await updateProfile({ resumes: updatedResumes })
      profile.value.resumes = updatedResumes
    } catch (err) {
      console.error('Ошибка при добавлении id резюме в профиль:', err)
    }
  }
  

  return {
    profile,
    fetchProfile,
    setMainResume,
    updateProfile,
    isEditing,
    editedProfile,
    startEdit,
    saveProfile,
    addResumeId,
  }
})
