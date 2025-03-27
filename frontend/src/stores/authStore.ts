import { ref } from 'vue'
import { defineStore } from 'pinia'
import { loginUser, registerUser } from '@/api/auth'
import type { UserRegisterDto } from '@/types/auth'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string>(localStorage.getItem('token') || '')
  const loading = ref(false)
  const error = ref('')

  const login = async (email: string, password: string) => {
    loading.value = true
    error.value = ''
    try {
      const result = await loginUser({ email, password })
      token.value = result
      localStorage.setItem('token', result)
    } catch (e: any) {
      error.value = e.response?.data || 'Login failed'
    } finally {
      loading.value = false
    }
  }

  const register = async (payload: UserRegisterDto) => {
    loading.value = true
    error.value = ''
    try {
      await registerUser(payload)
    } catch (e: any) {
      error.value = e.response?.data || 'Registration failed'
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
