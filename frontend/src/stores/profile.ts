//profile.ts
import api from "@/api"
import { Profile } from "@/types/profile"
import { defineStore } from "pinia"
import { ref } from "vue"

export const useProfileStore = defineStore('profile', () => {
  const profile = ref<Profile | null>(null)
  const isEditing = ref(false)
  const editedProfile = ref<Pick<Profile, 'name' | 'email'>>({ name: '', email: '' })

  const fetchProfile = async () => {
    try {
      const profileRes = await api.get('/profile')
      profile.value = profileRes.data
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

  return {
    profile,
    fetchProfile,
    setMainResume,
    updateProfile,
    isEditing,
    editedProfile,
    startEdit,
    saveProfile
  }
})
