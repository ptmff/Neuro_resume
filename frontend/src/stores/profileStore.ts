import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '@/api'

export interface Education {
  institution: string
  degree: string
  field: string
  startYear: number
  endYear: number
}

export interface Profile {
  email: string
  name?: string
  phone?: string
  city?: string
  photo?: string
  education?: Education[]
  mainResumeId?: number | null
}

export const useProfileStore = defineStore('profile', () => {
  const profile = ref<Profile | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const fetchProfile = async () => {
    try {
      loading.value = true
      const res = await api.get('/Profile')
      profile.value = { ...res.data }
    } catch (err: any) {
      error.value = err.response?.data || 'Ошибка загрузки профиля'
    } finally {
      loading.value = false
    }
  }

  const updateProfile = async (updatedData: Partial<Profile>) => {
    try {
      const res = await api.patch('/Profile', updatedData)
      profile.value = res.data
    } catch (err: any) {
      error.value = err.response?.data || 'Ошибка обновления профиля'
    }
  }

  const uploadPhoto = async (file: File) => {
    try {
      const formData = new FormData()
      formData.append('file', file)

      const res = await api.post('/Profile/photo', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })

      if (profile.value && res.data?.photo) {
        profile.value.photo = res.data.photo
      }
    } catch (err: any) {
      error.value = err.response?.data || 'Ошибка загрузки фото'
    }
  }

  const deleteProfile = async () => {
    try {
      await api.delete('/Profile')
      profile.value = null
    } catch (err: any) {
      error.value = err.response?.data || 'Ошибка удаления профиля'
    }
  }

  const clearProfile = () => {
    profile.value = null
    error.value = null
  }

  const setMainResume = (id: number | null) => {
    if (profile.value) {
      profile.value.mainResumeId = id
    }
  }

  return {
    profile,
    loading,
    error,
    fetchProfile,
    updateProfile,
    uploadPhoto,
    deleteProfile,
    clearProfile,
    setMainResume,
  }
})
