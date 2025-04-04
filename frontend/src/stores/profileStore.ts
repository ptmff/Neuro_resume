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
  emailVerified?: boolean
  phoneVerified?: boolean
}

export const useProfileStore = defineStore('profile', () => {
  const profile = ref<Profile | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)
  const verificationLoading = ref({
    email: false,
    phone: false
  })
  const verificationError = ref({
    email: null as string | null,
    phone: null as string | null
  })

  const fetchProfile = async () => {
    try {
      loading.value = true
      const res = await api.get('/Profile')
      profile.value = res.data
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
  
      if (profile.value && res.data?.photoUrl) {
        profile.value = {
          ...profile.value,
          photo: res.data.photoUrl,
        }
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

  /**
   * Send email verification request to the server
   * @returns Promise that resolves when verification email is sent
   */
  const sendEmailVerification = async () => {
    if (!profile.value?.email) {
      verificationError.value.email = 'Email не указан'
      return
    }

    try {
      verificationLoading.value.email = true
      verificationError.value.email = null
      
      // API call to send verification email
      await api.post('/Profile/verify-email')
      
      return true
    } catch (err: any) {
      verificationError.value.email = err.response?.data || 'Ошибка отправки верификации на email'
      throw err
    } finally {
      verificationLoading.value.email = false
    }
  }

  /**
   * Send phone verification SMS to the user's phone
   * @returns Promise that resolves when verification SMS is sent
   */
  const sendPhoneVerification = async () => {
    if (!profile.value?.phone) {
      verificationError.value.phone = 'Телефон не указан'
      return
    }

    try {
      verificationLoading.value.phone = true
      verificationError.value.phone = null
      
      // API call to send verification SMS
      await api.post('/Profile/verify-phone')
      
      return true
    } catch (err: any) {
      verificationError.value.phone = err.response?.data || 'Ошибка отправки SMS-кода'
      throw err
    } finally {
      verificationLoading.value.phone = false
    }
  }

  /**
   * Verify phone with SMS code
   * @param code The verification code received via SMS
   * @returns Promise that resolves when phone is verified
   */
  const verifyPhoneWithCode = async (code: string) => {
    try {
      verificationLoading.value.phone = true
      verificationError.value.phone = null
      
      // API call to verify phone with code
      const res = await api.post('/Profile/verify-phone-code', { code })
      
      // Update profile with verified status
      if (profile.value) {
        profile.value.phoneVerified = true
      }
      
      return true
    } catch (err: any) {
      verificationError.value.phone = err.response?.data || 'Неверный код подтверждения'
      throw err
    } finally {
      verificationLoading.value.phone = false
    }
  }

  /**
   * Verify email with token (usually called after user clicks link in email)
   * @param token The verification token from email link
   * @returns Promise that resolves when email is verified
   */
  const verifyEmailWithToken = async (token: string) => {
    try {
      verificationLoading.value.email = true
      verificationError.value.email = null
      
      // API call to verify email with token
      const res = await api.post('/Profile/verify-email-token', { token })
      
      // Update profile with verified status
      if (profile.value) {
        profile.value.emailVerified = true
      }
      
      return true
    } catch (err: any) {
      verificationError.value.email = err.response?.data || 'Ошибка подтверждения email'
      throw err
    } finally {
      verificationLoading.value.email = false
    }
  }

  return {
    profile,
    loading,
    error,
    verificationLoading,
    verificationError,
    fetchProfile,
    updateProfile,
    uploadPhoto,
    deleteProfile,
    clearProfile,
    setMainResume,
    sendEmailVerification,
    sendPhoneVerification,
    verifyPhoneWithCode,
    verifyEmailWithToken
  }
})