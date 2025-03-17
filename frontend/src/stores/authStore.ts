import { defineStore } from 'pinia'
import axios from 'axios'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '',
  }),

  actions: {
    setToken(token: string) {
      this.token = token
      localStorage.setItem('token', token)
    },

    logout() {
      this.token = ''
      localStorage.removeItem('token')
    },

    async login({ email, password }: { email: string; password: string }): Promise<boolean> {
      try {
        const response = await axios.post('/api/Auth/login', { email, password })
        this.setToken(response.data)
        return true
      } catch (err) {
        console.error('Login failed:', err)
        return false
      }
    },
  },
})
