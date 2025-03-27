<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const email = ref('')
const password = ref('')
const name = ref('')
const phone = ref('')
const city = ref('')

const auth = useAuthStore()
const router = useRouter()

const handleRegister = async () => {
  await auth.register({
    email: email.value,
    password: password.value,
    name: name.value,
    phone: phone.value,
    city: city.value,
  })

  if (!auth.error) {
    router.push('/login')
  }
}
</script>

<template>
  <div class="fixed z-[999] top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-full max-w-md p-8 rounded-xl shadow-xl bg-white dark:bg-neutral-800">
    <h2 class="text-2xl font-semibold text-center text-neutral-800 dark:text-white mb-6">
      Регистрация
    </h2>

    <form @submit.prevent="handleRegister" class="space-y-4">
      <input
        v-model="email"
        type="email"
        placeholder="Email"
        class="w-full px-4 py-2 border rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
        required
      />

      <input
        v-model="password"
        type="password"
        placeholder="Пароль"
        class="w-full px-4 py-2 border rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
        required
      />

      <input
        v-model="name"
        type="text"
        placeholder="Имя (необязательно)"
        class="w-full px-4 py-2 border rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
      />

      <input
        v-model="phone"
        type="text"
        placeholder="Телефон (необязательно)"
        class="w-full px-4 py-2 border rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
      />

      <input
        v-model="city"
        type="text"
        placeholder="Город (необязательно)"
        class="w-full px-4 py-2 border rounded-md bg-white dark:bg-neutral-700 text-black dark:text-white"
      />

      <button
        type="submit"
        :disabled="auth.loading"
        class="w-full bg-green-600 hover:bg-green-700 text-white font-semibold py-2 rounded-md transition-colors duration-200"
      >
        {{ auth.loading ? 'Регистрация...' : 'Зарегистрироваться' }}
      </button>
    </form>

    <p v-if="auth.error" class="text-red-500 text-sm mt-4 text-center">
      {{ auth.error }}
    </p>

    <p class="text-sm text-center mt-4 text-neutral-600 dark:text-neutral-300">
      Уже есть аккаунт?
      <router-link to="/login" class="text-blue-600 hover:underline">Войти</router-link>
    </p>
  </div>
</template>
