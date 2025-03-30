import { ref } from 'vue'
import { defineStore } from 'pinia'
import { loginUser, registerUser } from '@/api/auth'
import type { UserRegisterDto } from '@/types/auth'

interface LoginPayload {
  email: string
  phone: string
  password: string
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string>(localStorage.getItem('token') || '')
  const loading = ref(false)
  const error = ref('')

  const login = async (payload: LoginPayload) => {
    loading.value = true
    error.value = ''
    try {
      const result = await loginUser(payload)
      token.value = result
      localStorage.setItem('token', result)
    } catch (e: any) {
      error.value = e.response?.data || 'Ошибка входа'
    } finally {
      loading.value = false
    }
  }

  const register = async (payload: UserRegisterDto) => {
    loading.value = true
    error.value = ''
    try {
      await registerUser(payload)
  
      const loginPayload = {
        email: payload.email || '',
        phone: payload.phone || '',
        password: payload.password,
      }
  
      const result = await loginUser(loginPayload)
      token.value = result
      localStorage.setItem('token', result)
    } catch (e: any) {
      error.value = e.response?.data || 'Ошибка регистрации'
    } finally {
      loading.value = false
    }
  }  

  const logout = () => {
    token.value = ''
    localStorage.removeItem('token')
  }

  return {
    token,
    loading,
    error,
    login,
    register,
    logout
  }
})
