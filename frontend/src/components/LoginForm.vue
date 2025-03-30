<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { useProfileStore } from '@/stores/profileStore'

const emailOrPhone = ref('')
const password = ref('')
const auth = useAuthStore()
const router = useRouter()
const profileStore = useProfileStore()

const handleLogin = async () => {
  const isEmail = emailOrPhone.value.includes('@')

  const payload = {
    email: isEmail ? emailOrPhone.value : '',
    phone: isEmail ? '' : emailOrPhone.value,
    password: password.value,
  }

  await auth.login(payload)

  if (!auth.error) {
    await profileStore.fetchProfile()
    router.push('/')
  }
}
</script>

<template>
  <div class="fixed z-[999] top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-full max-w-md p-8 rounded-xl shadow-xl bg-white dark:bg-neutral-800">
    <h2 class="text-2xl font-semibold text-center text-neutral-800 dark:text-white mb-6">
      Войти в аккаунт
    </h2>

    <form @submit.prevent="handleLogin" class="space-y-4">
      <input
        v-model="emailOrPhone"
        type="text"
        placeholder="Email или телефон"
        class="w-full px-4 py-2 border border-neutral-300 dark:border-neutral-700 rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
        required
      />

      <input
        v-model="password"
        type="password"
        placeholder="Пароль"
        class="w-full px-4 py-2 border border-neutral-300 dark:border-neutral-700 rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
        required
      />

      <button
        type="submit"
        :disabled="auth.loading"
        class="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded-md transition-colors duration-200"
      >
        {{ auth.loading ? 'Вход...' : 'Войти' }}
      </button>
    </form>

    <p v-if="auth.error" class="text-red-500 text-sm mt-4 text-center">
      {{ auth.error }}
    </p>

    <p class="text-sm text-center mt-4 text-neutral-600 dark:text-neutral-300">
      Нет аккаунта?
      <router-link to="/register" class="text-blue-600 hover:underline">Зарегистрироваться</router-link>
    </p>
  </div>
</template>
